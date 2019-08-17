CREATE TABLE Skills
(
  ID INT AUTO_INCREMENT PRIMARY KEY,
  ParentID INT NULL,
  CharacterTechnicalName VARCHAR(32) NOT NULL,
  TechnicalName VARCHAR(32) NOT NULL,
  DisplayName VARCHAR(32) NOT NULL,
  Type ENUM('Ability', 'Major', 'Minor') NOT NULL,
  Description VARCHAR(1024) NOT NULL,
  LevelRequirement TINYINT NOT NULL,
  MinimumPoints TINYINT NOT NULL,
  MaximumPoints TINYINT NOT NULL,

  CONSTRAINT Skills_CharacterTechnicalName_TechnicalName_uindex UNIQUE (CharacterTechnicalName, TechnicalName),
  CONSTRAINT Skills_Skills_ID_fk FOREIGN KEY (ParentID) REFERENCES Skills (ID),
  CONSTRAINT Skills_Characters_Technicalname_fk FOREIGN KEY (CharacterTechnicalName) REFERENCES Characters (TechnicalName)
);

INSERT INTO Skills (ParentID, CharacterTechnicalName, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints) VALUES
  (null, 'anya', 'whiplash', 'Whiplash', 'Ability', 'A furious long range whip combo', 1, 1, 1),
  (1, 'anya', 'bloodboil-explosion', 'Bloodboil Explosion', 'Major', 'Last combo hit has 20% chance to cause enemies to explode when dying for 100% physical damage in 3m radius.', 1, 0, 1),
  (1, 'anya', 'quick-fix', 'Quick Fix', 'Major', 'Each Combo hit reduces Vitality Rush cooldown by 0.3s', 1, 0, 1),
  (1, 'anya', 'blood-clot', 'Blood Clot', 'Major', 'Last combo hit has 20% chance to spawn blood fragment.', 2, 0, 1),
  (null, 'anya', 'bloodsucker', 'Bloodsucker', 'Ability', 'Send out a huge blood bat that inflicts damage to all enemies it passes through and then returns to the caster, healing it for every enemy hit.', 2, 0, 3);
