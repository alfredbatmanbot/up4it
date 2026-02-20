# Up4It

**Coordinate hangouts without the coordination tax.**

## What Is It?

Up4It solves a real problem: texting 6 friends to see who's down for pizza tonight sucks. Half don't respond for hours, three have excuses, and by the time you hear back, it's 10 PM.

Instead:
1. Post "Pizza at Lou's tonight, 7 PM"
2. Friends tap "I'm in" or "Can't make it"
3. Done

No excuses to collect, no guilt-tripping, no endless group chat threads.

## Key Features (Planned)

- **Spontaneous events** - "Who's down for dinner tonight?"
- **Max attendance limits** - "Boat holds 5 people max"
- **Reliability tracking** - Flaky friends get auto opt-out after repeated no-shows
- **"What's happening tonight?"** - See what your friends are doing and join
- **Smart invites** - Random selection, ordered invites, open to everyone
- **Push + SMS notifications** - Reach people where they are

## Current Status

**✅ MVP Built:**
- Basic .NET MAUI app structure
- Supabase backend (PostgreSQL + real-time)
- Create events
- Event feed (what's happening today)
- Data model for users, events, invites, friendships

**🚧 Coming Next:**
- Phone authentication (Supabase Auth)
- Invite friends (search by phone)
- RSVP flow (accept/decline)
- Push notifications (FCM/APNS)
- Reliability tracking
- UI polish

## Tech Stack

**Frontend:**
- .NET MAUI (iOS + Android)
- C# / XAML
- MVVM pattern (CommunityToolkit.Mvvm)

**Backend:**
- Supabase (PostgreSQL + real-time + auth + storage)
- Row-level security for data privacy
- Auto-generated REST API

**Notifications:**
- Firebase Cloud Messaging (Android)
- Apple Push Notification Service (iOS)
- Twilio (SMS fallback)

## Project Structure

```
Up4It/
├── Models/          # Data models (Event, Profile, EventInvite)
├── Services/        # Business logic (SupabaseService)
├── ViewModels/      # MVVM ViewModels
├── Pages/           # UI screens (XAML + code-behind)
├── Resources/       # Fonts, images, styles
└── Platforms/       # iOS/Android specific code

supabase-schema.sql  # Database schema
SETUP.md            # Step-by-step setup guide
README.md           # This file
```

## Getting Started

1. **Follow SETUP.md** - Complete setup guide with Supabase + .NET MAUI
2. **Build the app** - `dotnet build`
3. **Run on iOS/Android** - See SETUP.md for commands
4. **Create your first event** - Pizza night, anyone?

## Design Philosophy

### 1. Low Friction
- No endless forms
- No required fields unless critical
- One-tap RSVP

### 2. Social Honesty
- It's okay to decline
- It's okay to ignore
- But show up if you say you will

### 3. Reliability Matters
- Track attendance patterns
- Auto opt-out flaky friends (with grace period)
- Earn trust back with consistency

### 4. Privacy First
- Friends-only by default
- No public social feed nonsense
- Your plans, your circle

## Why This Might Work

**The psychological insight:** People hate saying no directly to a friend's face (text). They'll ghost the group chat instead. Up4It removes the guilt by making decline/ignore a normal, non-confrontational action.

**The network effect:** Once 5-10 of your friends are on it, it becomes the default way to coordinate. "Check Up4It" replaces "Let me text everyone."

**The reliability angle:** Flaky friends get auto-filtered over time, so event creators stop wasting time on people who never show. It's a credit score for hangouts.

## Open Questions

- How many no-shows = auto opt-out? (Thinking 3-5)
- Should we show reliability scores publicly? (Leaning no, too judgey)
- Can you re-opt-in after auto opt-out? (Leaning yes, with explanation)
- Payment splitting integration? (Maybe later, out of scope for MVP)
- Discovery beyond friends? (Probably not, privacy risk)

## Contributing

Right now this is a prototype/learning project. If you want to build it with me, hit me up.

## License

TBD (probably MIT)

---

**Built with 🍕 by Clint + Alfred**
