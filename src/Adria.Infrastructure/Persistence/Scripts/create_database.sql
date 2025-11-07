DROP TABLE IF EXISTS `PointsPayment`;
DROP TABLE IF EXISTS `CosmeticItem`;
DROP TABLE IF EXISTS `CustomerCourse`;
DROP TABLE IF EXISTS `CourseModule`;
DROP TABLE IF EXISTS `Courses`;
DROP TABLE IF EXISTS `Payment`;
DROP TABLE IF EXISTS `CustomerSubscription`;
DROP TABLE IF EXISTS `Subscription`;
DROP TABLE IF EXISTS `User`;

/* CREATE TABLES */

-- USERS
CREATE TABLE `User` (
    `AdriaID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    `Wallet` DECIMAL(10,2) NOT NULL DEFAULT 0,
    `PointsBalance` INT DEFAULT 0,
    `LastPayment` DATE,
    `Streak` INT DEFAULT 0
);

-- SUBSCRIPTIONS
CREATE TABLE `Subscription` (
    `SubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type` VARCHAR(50) NOT NULL,
    `PricePerCycle` DECIMAL(10,2) NOT NULL
);

-- CUSTOMER SUBSCRIPTIONS
CREATE TABLE `CustomerSubscription` (
    `CustomerSubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `StartDate` DATETIME NOT NULL,
    `EndDate` DATETIME NOT NULL,
    `Status` VARCHAR(50) NOT NULL,
    `AdriaID` VARCHAR(36) NOT NULL,
    `SubscriptionID` VARCHAR(36) NOT NULL,
    
    FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`),
    FOREIGN KEY (`SubscriptionID`) REFERENCES `Subscription`(`SubscriptionID`)
);

-- PAYMENTS
CREATE TABLE `Payment` (
   `TransactionID` VARCHAR(36) PRIMARY KEY NOT NULL,
   `AdriaCoinAmount` DECIMAL(10,2) NOT NULL,
   `TransactionDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
   `AdriaID` VARCHAR(36) NOT NULL,
    
   FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`)
);

-- COURSES
CREATE TABLE `Courses` (
  `CourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
  `CourseName` VARCHAR(255) NOT NULL,
  `CourseDifficulty`INT NOT NULL,
  `CourseDescription` TEXT,
  `CourseCategory` VARCHAR(255) NOT NULL,
  `PointsOnCompletion` INT NOT NULL,
  `EstimatedDurationInHours` INT,
  `CoverImageID` VARCHAR(255),
  `BannerImageID` VARCHAR(255),
  `SubscriptionRequired` INT NOT NULL,
  `CourseTags` VARCHAR(255) NOT NULL
);

-- COURSE MODULES
CREATE TABLE `CourseModule` (
    `CourseModuleID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `ModuleName` VARCHAR(255) NOT NULL,
    `CourseID` VARCHAR(36) NOT NULL,
    FOREIGN KEY (`CourseID`) REFERENCES `Courses`(`CourseID`)
);

-- CUSTOMER COURSES
CREATE TABLE `CustomerCourse` (
  `CustomerCourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
  `CompletionStatus` BOOLEAN NOT NULL,
  `ProgressPercentage` INT NOT NULL DEFAULT 0,
  `StartDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CompletionDate` DATETIME,
  `AdriaID` VARCHAR(36) NOT NULL,
  `CourseID` VARCHAR(36) NOT NULL,
  FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`),
  FOREIGN KEY (`CourseID`) REFERENCES `Courses`(`CourseID`)
);

-- COSMETICS
CREATE TABLE `CosmeticItem` (
    `CosmeticItemID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `ItemName` VARCHAR(100) NOT NULL,
    `ItemType` VARCHAR(50) NOT NULL,
    `PointsCost` INT NOT NULL
);

-- POINT PAYMENTS
CREATE TABLE `PointsPayment` (
    `PointsTransactionID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `PointsAmount` INT NOT NULL,
    `TransactionDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `AdriaID` VARCHAR(36) NOT NULL,
    `CosmeticItemID` VARCHAR(36) NOT NULL,
    FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`),
    FOREIGN KEY (`CosmeticItemID`) REFERENCES `CosmeticItem`(`CosmeticItemID`)
);

/* SEED DATA */

-- USERS
INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('8fcff41a-0342-4f57-a12f-3e771a6dcea1', 'John');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('c3d8b6a4-6a9a-4df0-9322-9b8c71f12a5d', 'Alice');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('1e5b9f79-7e4a-4b48-9f77-932dd8fda47d', 'Bob');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('e92b7b45-78a0-44a2-8d89-2b3ad3ad28e3', 'Charlie');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('fa2a4b61-3c3b-44cf-b7d2-88b3e2fd1a1d', 'Diana');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('6b4e02e9-46c1-45c5-8c1b-1cb4e84c0f93', 'Ethan');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('b5d0ef9d-8a7c-4a6c-bc29-0f4cf60b2e60', 'Fiona');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('a0b2b58e-df83-49d3-b39b-1cded5b2b1b1', 'George');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('7efb3a55-4b6a-4af1-bab6-fbe6c9a52b94', 'Hannah');

INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES ('2dfb674e-78f0-487b-9268-6308b2df99f2', 'Ian');


-- SUBSCRIPTIONS
INSERT INTO `Subscription` (`SubscriptionID`, `Type`, `PricePerCycle`)
    VALUES (UUID(), 'Basic', 15.99);

INSERT INTO `Subscription` (`SubscriptionID`, `Type`, `PricePerCycle`)
    VALUES (UUID(), 'Standard', 22.99);

INSERT INTO `Subscription` (`SubscriptionID`, `Type`, `PricePerCycle`)
    VALUES (UUID(), 'Premium', 28.99);

INSERT INTO `Subscription` (`SubscriptionID`, `Type`, `PricePerCycle`)
    VALUES (UUID(), 'Ultra', 35.99);

-- COURSE
INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'A complete guide to cats',
           2,
           'This course explores feline history, biology, and behavior, offering a comprehensive look at domestic and wild cats. Students will learn about communication, health, and human-cat relationships. Practical sections focus on responsible care and understanding cat psychology.',
           'Animal Studies',
           450,
           8,
           1,
           'cats,animals,biology,behavior,pet care'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'How to hunt',
           1,
           'Learn the cultural, historical, and ecological perspectives surrounding hunting practices across civilizations. This course emphasizes ethical considerations, wildlife conservation, and safe equipment handling theory.',
           'Survival',
           250,
           12,
           2,
           'hunting,ethics,conservation,wildlife,outdoors'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Cleaning 101',
           0,
           'Discover foundational cleaning principles for home, office, and personal spaces. This course covers organization strategies, essential cleaning methods, and choosing safe, effective products.',
           'Lifestyle',
           125,
           4,
           0,
           'cleaning,organization,home,habits,productivity'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Wilderness Basics',
           1,
           'An introduction to the fundamental skills and knowledge required to safely experience the outdoors. Students will explore navigation, shelter concepts, weather awareness, and responsible environmental practices.',
           'Survival',
           550,
           16,
           2,
           'wilderness,outdoors,navigation,survival skills,nature'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'How to Mine',
           2,
           'Learn about the history, science, and environmental impact of mining across the world. This course covers mineral types, geological processes, and mining technologies at a conceptual level.',
           'Earth and Environment',
           450,
           10,
           3,
           'mining,geology,earth science,resources,sustainability,needed in adria'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Fishing 101',
           1,
           'Study the basics of aquatic ecosystems and the cultural importance of fishing traditions. Students learn about fish species, ethical angling principles, and environmental stewardship.',
           'Survival',
           250,
           8,
           1,
           'fishing,ecology,aquatic life,conservation,outdoors,needed in adria'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Basic Culinary Skills',
           0,
           'A beginner-friendly introduction to essential cooking methods and kitchen fundamentals. Students will learn knife safety theory, cooking terminology, and how to follow recipes confidently.',
           'Culinary Arts',
           225,
           4,
           0,
           'cooking,culinary,beginners,recipes,kitchen'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'How to Raise Children',
           3,
           'Explore developmental stages, emotional needs, and supportive caregiving approaches in childhood. The course draws from psychology, family studies, and cultural perspectives on parenting.',
           'Parenting & Psychology',
           300,
           6,
           1,
           'parenting,psychology,child development,communication,caregiving,needed in adria'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'How To Train Your Dragon',
           0,
           'A playful, fantasy-based exploration of dragon care inspired by mythology and literature. Students learn world-building, creature-design theory, and storytelling techniques.',
           'Mythology & Literature',
           100,
           4,
           0,
           'fantasy,dragons,mythology,storytelling,creativity'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Bird Blindness: A Deep Dive',
           2,
           'Examine the scientific, neurological, and perceptual mechanisms behind species-specific vision limitations—both real and theoretical.',
           'Science & Biology',
           450,
           8,
           2,
           'biology,vision,perception,neuroscience,evolution'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Psych 101',
           1,
           'An introduction to the major theories, research methods, and thinkers in psychology.',
           'Psychology',
           500,
           16,
           3,
           'psychology,behavior,cognition,research,mental health,needed in adria'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'King Von-ism: A History',
           2,
           'Study the cultural influence, storytelling style, and legacy of rapper King Von within modern hip-hop history.',
           'Music & Culture',
           800,
           9,
           3,
           'music,hip hop,culture,history,storytelling'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Surgery for Dummies',
           2,
           'A high-level overview of surgical history, ethics, and medical advances—strictly conceptual and informational.',
           'Medical Science',
           1200,
           48,
           3,
           'surgery,medicine,history,ethics,science,needed in adria'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Baking Bread for a Modern Household',
           2,
           'Learn the science and art of bread-making adapted for everyday kitchens.',
           'Culinary Arts',
           150,
           4,
           1,
           'baking,bread,cooking,food science,culinary'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'World History',
           2,
           'A sweeping journey through major civilizations, global events, and cultural developments from ancient to modern times.',
           'History',
           200,
           16,
           2,
           'history,civilizations,culture,politics,global'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Primates and Pizza',
           0,
           'An interdisciplinary course exploring primate behavior alongside cultural food anthropology—especially pizza''s fascinating global evolution.',
           'Anthropology & Food Studies',
           100,
           6,
           0,
           'primates,food,culture,anthropology,biology'
       );

INSERT INTO `Courses` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`, `CourseCategory`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`, `CourseTags`
)
VALUES (
           UUID(),
           'Intro to Bricklaying',
           2,
           'Learn the essential concepts of bricklaying, including materials, tools, and foundational construction techniques.',
           'Trades & Construction',
           325,
           4,
           1,
           'construction,bricklaying,skills,trade,foundation,needed in adria'
       );

-- COURSE MODULE
INSERT INTO `CourseModule` (`CourseModuleID`, `ModuleName`, `CourseID`)
    VALUES (
           UUID(),
           'Lesson 1 - Frying',
           (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'Basic culinary skills')
       );

-- CustomerCourse
INSERT INTO `CustomerCourse` (`CustomerCourseID`, `CompletionStatus`, `ProgressPercentage`, `StartDate`, `AdriaID`, `CourseID`)
VALUES
    (UUID(), FALSE, 20, NOW(), '8fcff41a-0342-4f57-a12f-3e771a6dcea1', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'A complete guide to cats')),
    (UUID(), TRUE, 100, NOW() - INTERVAL 10 DAY, '8fcff41a-0342-4f57-a12f-3e771a6dcea1', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'Basic Culinary Skills')),
    (UUID(), FALSE, 50, NOW() - INTERVAL 5 DAY, '8fcff41a-0342-4f57-a12f-3e771a6dcea1', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'Fishing 101'));

INSERT INTO `CustomerCourse` (`CustomerCourseID`, `CompletionStatus`, `ProgressPercentage`, `StartDate`, `AdriaID`, `CourseID`)
VALUES
    (UUID(), FALSE, 40, NOW() - INTERVAL 3 DAY, 'c3d8b6a4-6a9a-4df0-9322-9b8c71f12a5d', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'Wilderness Basics')),
    (UUID(), FALSE, 0, NOW(), 'c3d8b6a4-6a9a-4df0-9322-9b8c71f12a5d', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'How to Mine')),
    (UUID(), TRUE, 100, NOW() - INTERVAL 15 DAY, 'c3d8b6a4-6a9a-4df0-9322-9b8c71f12a5d', (SELECT `CourseID` FROM `Courses` WHERE `CourseName` = 'Primates and Pizza'));


-- COSMETIC ITEM
INSERT INTO `CosmeticItem` (`CosmeticItemID`, `ItemName`, `ItemType`, `PointsCost`)
    VALUES (UUID(), 'Spooky time', 'Frame', 500);
