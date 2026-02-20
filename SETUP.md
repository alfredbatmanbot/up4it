# Up4It - Setup Guide

## Part 1: Supabase Setup

### 1. Create Supabase Account
1. Go to [https://supabase.com](https://supabase.com)
2. Click "Start your project"
3. Sign in with GitHub (recommended)

### 2. Create Project
1. Click "New Project"
2. Choose organization (or create one)
3. Fill in:
   - **Name:** `up4it` (or whatever you want)
   - **Database Password:** (save this somewhere secure)
   - **Region:** Choose closest to you
   - **Pricing Plan:** Free tier is perfect for now
4. Click "Create new project"
5. Wait 2-3 minutes for setup

### 3. Get Your Credentials
1. Once project is ready, click **Settings** (gear icon on left sidebar)
2. Click **API** in settings menu
3. You'll see:
   - **Project URL** → This is your `SUPABASE_URL`
   - **anon public** key → This is your `SUPABASE_KEY`
4. **Save these somewhere** - you'll need them in a minute

### 4. Run the SQL Schema
1. Click **SQL Editor** (lightning bolt icon on left sidebar)
2. Click **+ New query**
3. Open `/workspace/up4it/supabase-schema.sql` (the file I created)
4. Copy the entire contents and paste into the SQL editor
5. Click **Run** (or press Cmd+Enter)
6. You should see "Success. No rows returned" - that's good!

### 5. Verify Tables
1. Click **Table Editor** (spreadsheet icon on left)
2. You should now see these tables:
   - `profiles`
   - `events`
   - `event_invites`
   - `friendships`
   - `reliability_logs`

If you see all 5 tables, you're good! ✅

---

## Part 2: Configure the .NET MAUI App

### 1. Set Environment Variables

**On macOS/Linux:**

Open terminal and add to your `~/.zshrc` (or `~/.bashrc`):

```bash
export SUPABASE_URL="https://your-project-id.supabase.co"
export SUPABASE_KEY="your-anon-key-here"
```

Then run:
```bash
source ~/.zshrc
```

**On Windows (PowerShell):**

```powershell
$env:SUPABASE_URL = "https://your-project-id.supabase.co"
$env:SUPABASE_KEY = "your-anon-key-here"
```

Or set permanently via System Settings → Environment Variables.

### 2. Verify Setup

Open terminal in the `Up4It` directory and run:

```bash
echo $SUPABASE_URL
echo $SUPABASE_KEY
```

You should see your credentials printed out.

---

## Part 3: Build and Run the App

### 1. Build the Project

```bash
cd /Users/openclaw/.openclaw/workspace/up4it/Up4It
dotnet build
```

If it builds successfully, you're ready to run!

### 2. Run on iOS Simulator (macOS only)

```bash
dotnet build -t:Run -f net10.0-ios
```

### 3. Run on Android Emulator

```bash
dotnet build -t:Run -f net10.0-android
```

### 4. Run on Physical iPhone (for TestFlight later)

You'll need:
- Apple Developer account (already set up)
- iPhone connected via cable
- Provisioning profile configured

```bash
dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64 -p:Configuration=Release
```

---

## Part 4: Test the App

### Test Flow

1. **App launches** → You should see the Event Feed (empty for now)
2. **Tap "Create Event"** → Fill in:
   - Title: "Pizza Night"
   - Location: "Lou Malnati's"
   - Start Time: Tonight at 7 PM
3. **Tap "Create Event"** → Should show "Success" alert
4. **Go back** → Event should appear in "My Events"

If that works, **you have a working prototype!** 🎉

---

## Troubleshooting

### Build Errors

**"SUPABASE_URL not configured"**
- Environment variables not set properly
- Re-run the export commands above
- Restart your terminal/IDE

**"Package not found" errors**
- Run `dotnet restore` in the Up4It folder
- Make sure you're in the project directory

**iOS build fails**
- Check Xcode is installed: `xcode-select --install`
- Open Xcode and accept license agreements
- Run `dotnet workload install maui-ios`

### Runtime Errors

**"Unable to connect to Supabase"**
- Check your `SUPABASE_URL` and `SUPABASE_KEY` are correct
- Make sure the Supabase project is running (not paused)
- Check your internet connection

**Events not showing**
- Open Supabase dashboard → Table Editor → `events` table
- Verify the event was inserted
- Check the `start_time` is today

---

## Next Steps

Once you have the basic app working:

1. **Add authentication** - Right now anyone can create events (we'll add phone auth next)
2. **Invite friends** - Add ability to search and invite users
3. **RSVP flow** - Accept/decline invites
4. **Push notifications** - Get notified when invited
5. **Polish UI** - Make it pretty!

Want to tackle any of these next?
