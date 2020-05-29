Public SYNCHOTRON
- Logging/Tracking can be injected by implementing ISynchotronLog and assigning it to Synchotron.GlobalLog
  - The tests use this to validate the run sequence

Internal RESOURCE is something that is tracked and flagged as serial or paralled, so it can be involved in the blocking logic
Internal SLOTS has the overview of the current Resources tracked

