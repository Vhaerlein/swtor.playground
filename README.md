# swtor.playground
SWtOR related stuff (stat optimization, log reader, etc...)

--

**StatOptimizer**

Application for searching optimal stat distribution for any dps class (theoretically, though tested only on combat sentinel).

- Alacrity is now GCD rounding aware (GCD is rounded to the highest 0.1s fraction, e.g. for 13% alacrity GCD will be 1,3274 and it will be rounded up to 1.4s)
- Adrenals/relics are not included in dps mathematical model.
- All parameters can be changed from UI (mastery, power, etc...).
- Save/Load profile (app will remember last used profile).
- Profile is Json file, if needed can be edited outside application (and for some parameters it's the only way).
- Load combat session data from combat log allowing you can do calculations theoretically for any dps class (ability name tooltip will show basic session information about this ability).
- Option to optimize stats vs single session and vs all sessions.
- SWtOR 5.2 actual abilities database (ability icon tooltip will show some of DB information).
- Stat formulas are located in App.config.
- Ability xml can be overriden with "xml\abilities.xml" file.

Some number tuning is required after data is loaded from combat log. E.g. sentinel's Ataru Form is a triggered ability, and in parsed data it will have like 1 activation and 180 hits. App doesn't do any guessing about triggered abilities so it's up to you to change then numbers (in this case change activation number from 1 to 180). Same is true for AoE abilities (e.g. Cyclone Slash is 2 hits per one activation, thus in-app activation number should be [total hits] divided by 2).

--

**ClassAbilityXmlMerger**

Quick and dirty class ability xml merger (for xml files obtained from torcommunity.com).
New updates should be placed in xml folder as well (just increase [00N] number prefix for each new update file).

--

Latest compiled version can be always found under "Releases" section.
