#!/bin/bash
# Run database migrations on Supabase

set -e

if [ -z "$1" ]; then
    echo "Usage: ./run-migration.sh <migration-file.sql>"
    exit 1
fi

MIGRATION_FILE="$1"

if [ ! -f "$MIGRATION_FILE" ]; then
    echo "Error: Migration file not found: $MIGRATION_FILE"
    exit 1
fi

# Load environment variables
source .env

echo "Running migration: $MIGRATION_FILE"

PGPASSWORD="$SUPABASE_DB_PASSWORD" psql "$DATABASE_URL" -f "$MIGRATION_FILE"

echo "✅ Migration completed successfully!"
