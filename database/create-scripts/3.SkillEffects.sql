CREATE TABLE SkillEffects
(
  SkillID INT NOT NULL,
  Level TINYINT NOT NULL,
  Sequence TINYINT NOT NULL,
  Description VARCHAR(1024) NOT NULL,

  CONSTRAINT SkillEffects_pk PRIMARY KEY (SkillID, Level, Sequence)
);

INSERT INTO SkillEffects (SkillID, Level, Sequence, Description) VALUES
  (5, 1, 1, 'Physical Damage 200%'),
  (5, 1, 2, 'Range 8m'),
  (5, 1, 3, 'Life Leech 3%'),
  (5, 2, 1, 'Physical Damage 250%'),
  (5, 2, 2, 'Range 8m'),
  (5, 2, 3, 'Life Leech 3%'),
  (5, 3, 1, 'Physical Damage 300%'),
  (5, 3, 2, 'Range 8m'),
  (5, 3, 3, 'Life Leech 3%');
