-- Up4It Database Schema for Supabase
-- Run this in the Supabase SQL Editor

-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Users table (extends Supabase auth.users)
CREATE TABLE public.profiles (
    id UUID REFERENCES auth.users ON DELETE CASCADE PRIMARY KEY,
    phone_number TEXT UNIQUE NOT NULL,
    display_name TEXT NOT NULL,
    email TEXT,
    profile_image_url TEXT,
    timezone TEXT DEFAULT 'America/Chicago',
    notification_preferences JSONB DEFAULT '{"push": true, "sms": false}'::jsonb,
    reliability_score INTEGER DEFAULT 100,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    last_active_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Events table
CREATE TABLE public.events (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    creator_id UUID REFERENCES public.profiles(id) ON DELETE CASCADE NOT NULL,
    title TEXT NOT NULL,
    description TEXT,
    start_time TIMESTAMP WITH TIME ZONE NOT NULL,
    end_time TIMESTAMP WITH TIME ZONE,
    location TEXT,
    max_attendees INTEGER,
    min_attendees INTEGER,
    visibility TEXT NOT NULL CHECK (visibility IN ('private', 'friends', 'public')) DEFAULT 'friends',
    status TEXT NOT NULL CHECK (status IN ('open', 'cancelled', 'full', 'completed')) DEFAULT 'open',
    rsvp_deadline TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Event Invites / RSVPs
CREATE TABLE public.event_invites (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    event_id UUID REFERENCES public.events(id) ON DELETE CASCADE NOT NULL,
    user_id UUID REFERENCES public.profiles(id) ON DELETE CASCADE NOT NULL,
    status TEXT NOT NULL CHECK (status IN ('pending', 'accepted', 'declined', 'ignored')) DEFAULT 'pending',
    invited_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    responded_at TIMESTAMP WITH TIME ZONE,
    notified_at TIMESTAMP WITH TIME ZONE,
    UNIQUE(event_id, user_id)
);

-- Friendships
CREATE TABLE public.friendships (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user1_id UUID REFERENCES public.profiles(id) ON DELETE CASCADE NOT NULL,
    user2_id UUID REFERENCES public.profiles(id) ON DELETE CASCADE NOT NULL,
    status TEXT NOT NULL CHECK (status IN ('pending', 'accepted', 'blocked')) DEFAULT 'pending',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    accepted_at TIMESTAMP WITH TIME ZONE,
    CHECK (user1_id < user2_id), -- Ensure consistent ordering
    UNIQUE(user1_id, user2_id)
);

-- Reliability Log (for tracking attendance)
CREATE TABLE public.reliability_logs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID REFERENCES public.profiles(id) ON DELETE CASCADE NOT NULL,
    event_id UUID REFERENCES public.events(id) ON DELETE CASCADE NOT NULL,
    action TEXT NOT NULL CHECK (action IN ('accepted_showed', 'accepted_noshow', 'declined')),
    logged_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX idx_events_creator ON public.events(creator_id);
CREATE INDEX idx_events_start_time ON public.events(start_time);
CREATE INDEX idx_events_status ON public.events(status);
CREATE INDEX idx_event_invites_user ON public.event_invites(user_id);
CREATE INDEX idx_event_invites_event ON public.event_invites(event_id);
CREATE INDEX idx_event_invites_status ON public.event_invites(status);
CREATE INDEX idx_friendships_user1 ON public.friendships(user1_id);
CREATE INDEX idx_friendships_user2 ON public.friendships(user2_id);

-- Row Level Security (RLS) Policies

-- Enable RLS on all tables
ALTER TABLE public.profiles ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.events ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.event_invites ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.friendships ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.reliability_logs ENABLE ROW LEVEL SECURITY;

-- Profiles: Users can read all profiles but only update their own
CREATE POLICY "Public profiles are viewable by everyone"
    ON public.profiles FOR SELECT
    USING (true);

CREATE POLICY "Users can update own profile"
    ON public.profiles FOR UPDATE
    USING (auth.uid() = id);

-- Events: Users can view events they're invited to or public events
CREATE POLICY "Users can view their events"
    ON public.events FOR SELECT
    USING (
        creator_id = auth.uid() OR
        visibility = 'public' OR
        (visibility = 'friends' AND EXISTS (
            SELECT 1 FROM public.friendships
            WHERE (user1_id = auth.uid() AND user2_id = creator_id)
               OR (user2_id = auth.uid() AND user1_id = creator_id)
        )) OR
        id IN (SELECT event_id FROM public.event_invites WHERE user_id = auth.uid())
    );

CREATE POLICY "Users can create events"
    ON public.events FOR INSERT
    WITH CHECK (auth.uid() = creator_id);

CREATE POLICY "Creators can update their events"
    ON public.events FOR UPDATE
    USING (auth.uid() = creator_id);

CREATE POLICY "Creators can delete their events"
    ON public.events FOR DELETE
    USING (auth.uid() = creator_id);

-- Event Invites: Users can see invites for their events or invites to them
CREATE POLICY "Users can view relevant invites"
    ON public.event_invites FOR SELECT
    USING (
        user_id = auth.uid() OR
        event_id IN (SELECT id FROM public.events WHERE creator_id = auth.uid())
    );

CREATE POLICY "Event creators can create invites"
    ON public.event_invites FOR INSERT
    WITH CHECK (
        event_id IN (SELECT id FROM public.events WHERE creator_id = auth.uid())
    );

CREATE POLICY "Users can update their own invite status"
    ON public.event_invites FOR UPDATE
    USING (user_id = auth.uid());

-- Friendships: Users can see their own friendships
CREATE POLICY "Users can view their friendships"
    ON public.friendships FOR SELECT
    USING (user1_id = auth.uid() OR user2_id = auth.uid());

CREATE POLICY "Users can create friendships"
    ON public.friendships FOR INSERT
    WITH CHECK (user1_id = auth.uid() OR user2_id = auth.uid());

CREATE POLICY "Users can update their friendships"
    ON public.friendships FOR UPDATE
    USING (user1_id = auth.uid() OR user2_id = auth.uid());

-- Reliability Logs: Users can only see their own
CREATE POLICY "Users can view their reliability logs"
    ON public.reliability_logs FOR SELECT
    USING (user_id = auth.uid());

-- Functions

-- Function to automatically create profile on signup
CREATE OR REPLACE FUNCTION public.handle_new_user()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO public.profiles (id, phone_number, display_name, email)
    VALUES (
        NEW.id,
        COALESCE(NEW.phone, NEW.email), -- Use phone if available, else email
        COALESCE(NEW.raw_user_meta_data->>'display_name', 'User'),
        NEW.email
    );
    RETURN NEW;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;

-- Trigger to create profile on new user
CREATE TRIGGER on_auth_user_created
    AFTER INSERT ON auth.users
    FOR EACH ROW
    EXECUTE FUNCTION public.handle_new_user();

-- Function to update updated_at timestamp
CREATE OR REPLACE FUNCTION public.update_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger to auto-update updated_at on events
CREATE TRIGGER update_events_updated_at
    BEFORE UPDATE ON public.events
    FOR EACH ROW
    EXECUTE FUNCTION public.update_updated_at();

-- View: Get events happening today/tonight
CREATE OR REPLACE VIEW public.events_today AS
SELECT e.*, p.display_name as creator_name, p.profile_image_url as creator_image,
    (SELECT COUNT(*) FROM public.event_invites WHERE event_id = e.id AND status = 'accepted') as attendee_count
FROM public.events e
JOIN public.profiles p ON e.creator_id = p.id
WHERE e.start_time::date = CURRENT_DATE
  AND e.status = 'open'
ORDER BY e.start_time;
