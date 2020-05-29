Nunit is set to run tests within a class sequentially, and run classes in parallel
- Within a class, the run order of tests is not as coded, so I had to set their ordering explicitly
- Must use release mode, as running Nunit in Debug mode only runs 1 test at a time
- The Global Test Run Hooks do not allow writing output, so I have had to add a test that gets run last
  "Thread3RunOnTick7EndOfRun" contains the logging/tracking output, and asserts that the log contains what is expected
- This only works if ALL the tests are run, as the original solution was for a the specflow automation framework in NUnit
- For fun, Parallel tests starting processing do a low beep, and when finished a slightly higher one, Sequential tests beep a fair bit higher, test results by ear experiment!

Synchronisation Algorithm
   Locking ensures that the synchronisation checks are serialised
   (S)erial test waits until no other tests are in (r)unning state
   (p)Parallel test waits until there are no serial tests at any state, eg (b)locked

State Table for the test implementation
- We use Time Ticks with State so we can control parallel test synchronisation
- Each test initialisation is postfixed with the ammount of ticks it runs for (after being unblocked)

+==+====+====+====+
|Ti|Thre|Thre|Thre|
|ck|ad1 |ad2 |ad3 |
+==+====+====+====+
|1 | Pr1| Pr2| Pr2|
|2 | Sb1|    |    |
|3 |    | Pb1| Sb1|
|4 | Sr |    |    |
|5 | Pb1|    | Sr |
|6 | Pr | Pr | Pr1|
+==+====+====+====+

Example Output from "Thread3RunOnTick7EndOfRun"

29/05/2020 17:18:15 +01:00  Total Slots 1 with Slot 0 [> P r] =  [> P r] Thread2RunOnTick1
29/05/2020 17:18:15 +01:00  Total Slots 2 with Slot 1 [> P r] =                               [> P r] Thread3RunTick1
29/05/2020 17:18:15 +01:00  Total Slots 3 with Slot 2 [> P r] =                                                             [> P r] Thread1RunOnTick1
29/05/2020 17:18:17 +01:00  Total Slots 3 with Slot 2 [< P r] =                                                             [< P r] Thread1RunOnTick1
29/05/2020 17:18:17 +01:00  Total Slots 3 with Slot 2 [> S w] =                                                             [> S w] Thread1RunOnTick4Sequential
29/05/2020 17:18:19 +01:00  Total Slots 3 with Slot 0 [< P r] =  [< P r] Thread2RunOnTick1
29/05/2020 17:18:19 +01:00  Total Slots 2 with Slot 1 [< P r] =                               [< P r] Thread3RunTick1
29/05/2020 17:18:19 +01:00  Total Slots 1 with Slot 2 [* S r] =                                                             [* S r] Thread1RunOnTick4Sequential
29/05/2020 17:18:19 +01:00  Total Slots 2 with Slot 0 [> P w] =  [> P w] Thread2RunOnTick6
29/05/2020 17:18:19 +01:00  Total Slots 3 with Slot 1 [> S w] =                               [> S w] Thread3RunOnTick5Sequential
29/05/2020 17:18:22 +01:00  Total Slots 3 with Slot 2 [< S r] =                                                             [< S r] Thread1RunOnTick4Sequential
29/05/2020 17:18:22 +01:00  Total Slots 2 with Slot 1 [* S r] =                               [* S r] Thread3RunOnTick5Sequential
29/05/2020 17:18:22 +01:00  Total Slots 3 with Slot 2 [> P w] =                                                             [> P w] Thread1RunOnTick6
29/05/2020 17:18:25 +01:00  Total Slots 3 with Slot 1 [< S r] =                               [< S r] Thread3RunOnTick5Sequential
29/05/2020 17:18:25 +01:00  Total Slots 2 with Slot 0 [* P r] =  [* P r] Thread2RunOnTick6
29/05/2020 17:18:25 +01:00  Total Slots 2 with Slot 2 [* P r] =                                                             [* P r] Thread1RunOnTick6
29/05/2020 17:18:25 +01:00  Total Slots 3 with Slot 1 [> P r] =                               [> P r] Thread3RunOnTick6
29/05/2020 17:18:27 +01:00  Total Slots 3 with Slot 2 [< P r] =                                                             [< P r] Thread1RunOnTick6
29/05/2020 17:18:27 +01:00  Total Slots 2 with Slot 0 [< P r] =  [< P r] Thread2RunOnTick6
29/05/2020 17:18:27 +01:00  Total Slots 1 with Slot 1 [< P r] =                               [< P r] Thread3RunOnTick6
29/05/2020 17:18:27 +01:00  Total Slots 1 with Slot 0 [> P r] =  [> P r] Thread3RunOnTick7EndOfRun
Line Scanning .....
    p1(1,0,2) s2(7) s3(11) p4(14,15,16)
