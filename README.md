# swtor.playground
SWtOR related stuff (stat optimization, log reader, etc...)

-- 

StatOptimizer:

Application searching optimal stat distribution for any dps class.

- Adrenals/relics are not included in dps mathematical model.
- All parameters can be changed from UI (mastery, power, etc...)
- Save/Load profile (app will remember last used profile)
- Profile is Json file, if needed can be edited outside application
- Load combat session data from combat log, meaning you can do calculations for any dps class (ability name tooltip will show basic session information about this ability).
- Option to optimize stats vs single session and vs all sessions
- SWtOR 4.0.2 actual abilities database (ability icon tooltip will show some of DB information)
- Accuracy is now also included in distribution

Some number tuning is required after data is loaded from combat log. E.g. sentinel's Ataru Form is a triggered ability, and in parse it will have like 1 activation and 180 hits. App doesn't do any guessing about triggered abilities so it's up to you to change numbers (in this case change activation number from 1 to 180). Same goes for AoE abilities (e.g. Cyclone Slash is 2 hits per one activation, thus in-app activation number should be [total hits] divided by 2).

--

StatDistributions:

Simple console app to build optimal stat distributions for budgets from 1000 to 10000 (assuming mastery points == all other points)

--

Latest compiled version can be always found under "Releases" section.