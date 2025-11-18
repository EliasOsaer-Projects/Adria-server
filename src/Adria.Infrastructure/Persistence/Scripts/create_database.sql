DROP TABLE IF EXISTS `PointsPayment`;
DROP TABLE IF EXISTS `UserQuestionAnswer`;
DROP TABLE IF EXISTS `QuestionAnswer`;
DROP TABLE IF EXISTS `Answer`;
DROP TABLE IF EXISTS `Question`;
DROP TABLE IF EXISTS `Module`;
DROP TABLE IF EXISTS `UserCourse`;
DROP TABLE IF EXISTS `UserSubscription`;
DROP TABLE IF EXISTS `Payment`;
DROP TABLE IF EXISTS `CosmeticItem`;

DROP TABLE IF EXISTS `Course`;
DROP TABLE IF EXISTS `CoverImage`;
DROP TABLE IF EXISTS `BannerImage`;
DROP TABLE IF EXISTS `Subscription`;
DROP TABLE IF EXISTS `User`;

/* CREATE TABLES */

-- USERS
CREATE TABLE `User` (
                        `AdriaID` VARCHAR(36) PRIMARY KEY NOT NULL,
                        `Name` VARCHAR(100) NOT NULL,
                        `Wallet` DECIMAL(10,2) NOT NULL DEFAULT 0,
                        `PointsBalance` INT DEFAULT 0,
                        `Streak` INT DEFAULT 0,
                        `ProfilePicture` LONGBLOB
);

-- SUBSCRIPTIONS
CREATE TABLE `Subscription` (
                                `SubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
                                `Type` VARCHAR(50) NOT NULL,
                                `Price` DECIMAL(10,2) NOT NULL,
                                `IsMostPopular` BOOLEAN NOT NULL DEFAULT FALSE,
                                `Description` VARCHAR(1024)
);

-- CUSTOMER SUBSCRIPTIONS
CREATE TABLE `UserSubscription` (
                                    `UserSubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
                                    `StartDate` DATETIME NOT NULL,
                                    `EndDate` DATETIME NOT NULL,
                                    `Status` VARCHAR(50) NOT NULL,
                                    `AdriaID` VARCHAR(36) NOT NULL,
                                    `SubscriptionID` VARCHAR(36) NOT NULL,

                                    FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`) ON DELETE CASCADE,
                                    FOREIGN KEY (`SubscriptionID`) REFERENCES `Subscription`(`SubscriptionID`) ON DELETE CASCADE
);

-- PAYMENTS
CREATE TABLE `Payment` (
                           `TransactionID` VARCHAR(36) PRIMARY KEY NOT NULL,
                           `AdriaCoinAmount` DECIMAL(10,2) NOT NULL,
                           `TransactionDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                           `AdriaID` VARCHAR(36) NOT NULL,

                           FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`) ON DELETE CASCADE
);

CREATE TABLE `BannerImage` (
                               `BannerImageID` VARCHAR(36) PRIMARY KEY NOT NULL,
                               `BannerImage` LONGBLOB
);

CREATE TABLE `CoverImage` (
                              `CoverImageID` VARCHAR(36) PRIMARY KEY NOT NULL,
                              `CoverImage` LONGBLOB
);

-- COURSES
CREATE TABLE `Course` (
                          `CourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
                          `CourseName` VARCHAR(255) NOT NULL,
                          `CourseDifficulty`INT NOT NULL,
                          `CourseDescription` VARCHAR(4096),
                          `CourseCategory` VARCHAR(255) NOT NULL,
                          `CourseTags` VARCHAR(255) NOT NULL,
                          `PointsOnCompletion` INT NOT NULL,
                          `EstimatedDurationInHours` INT,
                          `CoverImageID` VARCHAR(36),
                          `BannerImageID` VARCHAR(36),
                          `LearnList` VARCHAR(1024),
                          `SubscriptionRequired` BOOLEAN NOT NULL,

                          FOREIGN KEY (`CoverImageID`) REFERENCES `CoverImage`(`CoverImageID`) ON DELETE CASCADE,
                          FOREIGN KEY (`BannerImageID`) REFERENCES `BannerImage`(`BannerImageID`) ON DELETE CASCADE
);

-- CUSTOMER COURSES
CREATE TABLE `UserCourse` (
                              `CompletionStatus` BOOLEAN NOT NULL DEFAULT 0,
                              `ProgressPercentage` INT NOT NULL DEFAULT 0,
                              `StartDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                              `EndDate` DATETIME,
                              `AdriaID` VARCHAR(36) NOT NULL,
                              `CourseID` VARCHAR(36) NOT NULL,
                              FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`) ON DELETE CASCADE,
                              FOREIGN KEY (`CourseID`) REFERENCES `Course`(`CourseID`) ON DELETE CASCADE
);

-- COURSE MODULES

CREATE TABLE `Module` (
                          `ModuleID` VARCHAR(36) PRIMARY KEY NOT NULL,
                          `CourseID` VARCHAR(36) NOT NULL,
                          `Position` INT NOT NULL,
                          `Content` TEXT NOT NULL,

                          FOREIGN KEY (`CourseID`) REFERENCES `Course`(`CourseID`) ON DELETE CASCADE
);

-- QUESTIONS AND ANSWERS

CREATE TABLE `Question` (
                            `QuestionID` VARCHAR(36) PRIMARY KEY NOT NULL,
                            `Question` VARCHAR(1024) NOT NULL
);

CREATE TABLE `Answer` (
                          `AnswerID` VARCHAR(36) PRIMARY KEY NOT NULL,
                          `Answer` VARCHAR(1024) NOT NULL,
                          `IsCorrect` BOOLEAN NOT NULL
);

CREATE TABLE `QuestionAnswer` (
                                  `QAID` VARCHAR(36) PRIMARY KEY NOT NULL,
                                  `ModuleID` VARCHAR(36) NOT NULL,
                                  `QuestionID` VARCHAR(36) NOT NULL,
                                  `AnswerID` VARCHAR(36),

                                  FOREIGN KEY (`ModuleID`) REFERENCES `Module`(`ModuleID`) ON DELETE CASCADE,
                                  FOREIGN KEY (`QuestionID`) REFERENCES `Question`(`QuestionID`) ON DELETE CASCADE,
                                  FOREIGN KEY (`AnswerID`) REFERENCES `Answer`(`AnswerID`) ON DELETE CASCADE
);

CREATE TABLE `UserQuestionAnswer` (
                                      `QAID` VARCHAR(36) NOT NULL,
                                      `AdriaID` VARCHAR(36) NOT NULL,
                                      `IsCorrectlyAnswered` BOOLEAN NOT NULL,

                                      FOREIGN KEY (`QAID`) REFERENCES `QuestionAnswer`(`QAID`) ON DELETE CASCADE,
                                      FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`) ON DELETE CASCADE
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
                                 FOREIGN KEY (`AdriaID`) REFERENCES `User`(`AdriaID`) ON DELETE CASCADE,
                                 FOREIGN KEY (`CosmeticItemID`) REFERENCES `CosmeticItem`(`CosmeticItemID`) ON DELETE CASCADE
);