# Up4It Roadmap

## ✅ Phase 1: MVP Foundation (DONE)

- [x] Define data model (Users, Events, Invites, Friendships)
- [x] Create Supabase SQL schema
- [x] Build .NET MAUI project structure
- [x] Integrate Supabase C# SDK
- [x] Create core models (Profile, Event, EventInvite)
- [x] Build SupabaseService (CRUD operations)
- [x] Create CreateEventViewModel + Page
- [x] Create EventFeedViewModel + Page
- [x] Set up dependency injection (MauiProgram)
- [x] Configure AppShell routing

---

## 🚧 Phase 2: Authentication & Friends (NEXT)

### Auth Flow
- [ ] Add LoginPage (phone number entry)
- [ ] Integrate Supabase phone auth (OTP)
- [ ] Store auth token securely (SecureStorage)
- [ ] Add auth check on app startup
- [ ] Handle auth state changes

### Friend Management
- [ ] Add FriendsPage (list, search, add)
- [ ] Search users by phone number
- [ ] Send friend requests
- [ ] Accept/decline requests
- [ ] Block users
- [ ] View friend profiles

**Deliverable:** Users can sign in with phone and add friends

---

## 🔨 Phase 3: Invites & RSVPs

### Invite Flow
- [ ] Select friends when creating event
- [ ] Send invites (create EventInvite records)
- [ ] Show invited count on event card
- [ ] Add EventDetailPage (full event view)

### RSVP Flow
- [ ] Accept/Decline buttons on EventDetailPage
- [ ] Update EventInvite status
- [ ] Show who's attending (attendee list)
- [ ] Handle max attendees (mark event full)
- [ ] Show "spots left" if max is set

### My Invites
- [ ] InvitesPage (pending invites)
- [ ] Badge count on nav icon
- [ ] Quick accept/decline from list

**Deliverable:** Full invite + RSVP loop working

---

## 📲 Phase 4: Notifications

### Push Setup
- [ ] Configure Firebase Cloud Messaging (Android)
- [ ] Configure Apple Push Notification Service (iOS)
- [ ] Store device tokens in profiles table
- [ ] Build notification sending service

### Notification Types
- [ ] Event invite received
- [ ] Someone accepted your event
- [ ] Event is full (creator notification)
- [ ] Event reminder (1 hour before)
- [ ] Friend request received

### SMS Fallback
- [ ] Integrate Twilio
- [ ] Send SMS if push fails
- [ ] Add SMS preferences to profile

**Deliverable:** Users get notified when invited

---

## 🎨 Phase 5: UI Polish

### Design System
- [ ] Define color palette
- [ ] Create custom button styles
- [ ] Add loading states everywhere
- [ ] Error handling + user-friendly messages
- [ ] Empty states with helpful CTAs

### Screens to Polish
- [ ] Event Feed - card design, animations
- [ ] Create Event - better date/time picker
- [ ] Event Detail - attendee avatars, map view
- [ ] Profile - edit profile, settings
- [ ] Friends List - search, status indicators

### Dark Mode
- [ ] Define dark theme colors
- [ ] Test all screens in dark mode

**Deliverable:** App looks and feels polished

---

## 📊 Phase 6: Reliability Tracking

### Data Collection
- [ ] Add "mark attendance" feature (post-event)
- [ ] Log shows/no-shows in reliability_logs
- [ ] Calculate reliability score (algorithm TBD)
- [ ] Update profile.reliability_score

### Auto Opt-Out
- [ ] Define thresholds (e.g., 3 no-shows in 30 days)
- [ ] Auto opt-out logic (set flag on profile)
- [ ] Show "opted out" status on invite UI
- [ ] Re-opt-in flow (explanation + manual action)

### Display
- [ ] Show reliability score on own profile only (private)
- [ ] Show attendance history (my stats)
- [ ] Optional: creator sees invitee reliability before sending

**Deliverable:** Flaky friends get filtered over time

---

## 🚀 Phase 7: Advanced Features

### Groups
- [ ] Create groups ("Poker Night Crew")
- [ ] Invite entire group to event
- [ ] Group chat? (maybe, TBD)

### Event Settings
- [ ] Random selection from group
- [ ] Ordered invites (invite 5, if 2 decline invite next 2)
- [ ] Open to friends-of-friends
- [ ] Public events (discoverable)

### "What's Happening"
- [ ] Show events happening today across your network
- [ ] Filter by time (now, tonight, this weekend)
- [ "Join" button for open events

### Calendar Integration
- [ ] Export event to iOS/Android calendar
- [ ] Sync attendee changes

**Deliverable:** Power user features unlocked

---

## 🧪 Phase 8: Testing & Beta

### Testing
- [ ] Unit tests for ViewModels
- [ ] Integration tests for SupabaseService
- [ ] UI tests (basic flows)

### Beta Launch
- [ ] TestFlight build (iOS)
- [ ] Closed beta with 10-20 users
- [ ] Collect feedback
- [ ] Fix critical bugs

### Performance
- [ ] Optimize SQL queries
- [ ] Add caching where needed
- [ ] Test at scale (100+ events)

**Deliverable:** Stable beta ready for wider use

---

## 🌍 Phase 9: Launch Prep

### Marketing Site
- [ ] Landing page explaining the concept
- [ ] Screenshots/video demo
- [ ] Sign up for early access

### App Store Prep
- [ ] Write app description
- [ ] Screenshots (iOS + Android)
- [ ] Privacy policy
- [ ] Terms of service
- [ ] Submit for review

### Analytics
- [ ] Add Mixpanel or similar
- [ ] Track key events (sign up, create event, RSVP)
- [ ] Monitor crash rates

**Deliverable:** Ready for public launch

---

## 📈 Phase 10: Post-Launch

### Iterate
- [ ] Monitor usage patterns
- [ ] User interviews
- [ ] Feature requests
- [ ] Bug fixes

### Scale
- [ ] Optimize Supabase costs
- [ ] Add more regions if needed
- [ ] Consider custom backend if scale requires

### Monetization (Maybe)
- [ ] Premium features? (TBD)
- [ ] Sponsored events? (TBD)
- [ ] Keep it free? (Ideal)

---

## Current Status: Phase 1 Complete ✅

**Next up:** Phase 2 (Auth + Friends)

Want to start on authentication next, or jump to another phase?
