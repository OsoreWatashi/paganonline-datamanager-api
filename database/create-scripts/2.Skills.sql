CREATE TABLE Skills
(
  ID INT AUTO_INCREMENT PRIMARY KEY,
  ParentID INT NULL,
  TechnicalName VARCHAR(32) NOT NULL,
  DisplayName VARCHAR(32) NOT NULL,
  Type ENUM('Ability', 'Major', 'Minor') NOT NULL,
  Description VARCHAR(1024) NOT NULL,
  LevelRequirement TINYINT NOT NULL,
  MinimumPoints TINYINT NOT NULL,
  MaximumPoints TINYINT NOT NULL,

  CONSTRAINT Skills_TechnicalName_uindex UNIQUE (TechnicalName),
  CONSTRAINT Skills_Skills_ID_fk FOREIGN KEY (ParentID) REFERENCES Skills (ID)
);

INSERT INTO Skills (ParentID, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints) VALUES
  (null, 'whiplash', 'Whiplash', 'Ability', 'A furious long range whip combo', 1, 1, 1),
  (1, 'bloodboil-explosion', 'Bloodboil Explosion', 'Major', 'Last combo hit has 20% chance to cause enemies to explode when dying for 100% physical damage in 3m radius.', 1, 0, 1),
  (1, 'quick-fix', 'Quick Fix', 'Major', 'Each Combo hit reduces Vitality Rush cooldown by 0.3s', 1, 0, 1),
  (1, 'blood-clot', 'Blood Clot', 'Major', 'Last combo hit has 20% chance to spawn blood fragment.', 2, 0, 1),
  (null, 'bloodsucker', 'Bloodsucker', 'Ability', 'Send out a huge blood bat that inflicts damage to all enemies it passes through and then returns to the caster, healing it for every enemy hit.', 2, 0, 3);
