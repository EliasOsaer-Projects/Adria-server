DROP TABLE IF EXISTS `PointsPayment`;
DROP TABLE IF EXISTS `CosmeticItem`;
DROP TABLE IF EXISTS `CustomerCourse`;
DROP TABLE IF EXISTS `CourseModule`;
DROP TABLE IF EXISTS `Course`;
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
CREATE TABLE `Course` (
  `CourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
  `CourseName` VARCHAR(255) NOT NULL,
  `CourseDifficulty` VARCHAR(50) NOT NULL,
  `CourseDescription` TEXT,
  `PointsOnCompletion` INT NOT NULL,
  `EstimatedDurationInHours` INT,
  `CoverImageID` VARCHAR(255),
  `BannerImageID` VARCHAR(255),
  `SubscriptionRequired` INT NOT NULL,
);

-- COURSE MODULES
CREATE TABLE `CourseModule` (
    `CourseModuleID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `ModuleName` VARCHAR(255) NOT NULL,
    `CourseID` VARCHAR(36) NOT NULL,
    FOREIGN KEY (`CourseID`) REFERENCES `Course`(`CourseID`)
);

-- CUSTOMER COURSES
CREATE TABLE `CustomerCourse` (
  `CustomerCourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
  `CompletionStatus` VARCHAR(50) NOT NULL,
  `ProgressPercentage` INT NOT NULL DEFAULT 0,
  `StartDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CompletionDate` DATETIME,
  `AdriaID` VARCHAR(36) NOT NULL,
  `CourseID` VARCHAR(36) NOT NULL,
  FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`),
  FOREIGN KEY (`CourseID`) REFERENCES `Course`(`CourseID`)
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

-- USER
INSERT INTO `User` (`AdriaID`, `Name`)
    VALUES (UUID(), 'TestUser');

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
INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
    VALUES (
           UUID(),
           'A complete guide to cats',
           'Hard',
           'This course explores feline history, biology, and behavior, offering a comprehensive look at domestic and wild cats. Students will learn about communication, health, and human-cat relationships. Practical sections focus on responsible care and understanding cat psychology.',
           450,
           8,
           1
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'How to hunt',
           'Moderate',
           'Learn the cultural, historical, and ecological perspectives surrounding hunting practices across civilizations. This course emphasizes ethical considerations, wildlife conservation, and safe equipment handling theory. Students will understand how hunting fits into ecosystems and responsible sourcing traditions—no real-world hunting skills are taught.',
           250,
           12,
           2
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'Cleaning 101',
           'Easy',
           'Discover foundational cleaning principles for home, office, and personal spaces. This course covers organization strategies, essential cleaning methods, and choosing safe, effective products. Hands-on examples help students develop sustainable cleaning habits.',
           125,
           4,
           0
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'Wilderness Basics',
           'Moderate',
           'An introduction to the fundamental skills and knowledge required to safely experience the outdoors. Students will explore navigation, shelter concepts, weather awareness, and responsible environmental practices. Emphasis is placed on safety, planning, and respecting nature.',
           550,
           16,
           2
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'How to Mine',
           'Hard',
           'Learn about the history, science, and environmental impact of mining across the world. This course covers mineral types, geological processes, and mining technologies at a conceptual level. Ethical and sustainable resource management is a key focus.',
           450,
           10,
           3
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'Fishing 101',
           'Moderate',
           'Study the basics of aquatic ecosystems and the cultural importance of fishing traditions. Students learn about fish species, ethical angling principles, and environmental stewardship. Techniques are discussed conceptually with a focus on conservation.',
           250,
           8,
           1
       );

INSERT INTO `Course` (
    `CourseID`, `CourseName`, `CourseDifficulty`, `CourseDescription`,
    `PointsOnCompletion`, `EstimatedDurationInHours`, `SubscriptionRequired`
)
VALUES (
           UUID(),
           'Basic Culinary Skills',
           'Easy',
           'A beginner-friendly introduction to essential cooking methods and kitchen fundamentals. Students will learn knife safety theory, cooking terminology, and how to follow recipes confidently. Emphasis is placed on flavor building and nutritious meal planning.',
           225,
           4,
           0
       );

-- COURSE MODULE
INSERT INTO `CourseModule` (`CourseModuleID`, `ModuleName`, `CourseID`)
    VALUES (
           UUID(),
           'Lesson 1 - Frying',
           (SELECT `CourseID` FROM `Course` WHERE `CourseName` = 'Basic culinary skills')
       );

-- COSMETIC ITEM
INSERT INTO `CosmeticItem` (`CosmeticItemID`, `ItemName`, `ItemType`, `PointsCost`)
    VALUES (UUID(), 'Spooky time', 'Frame', 500);