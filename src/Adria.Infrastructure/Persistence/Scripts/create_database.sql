DROP TABLE IF EXISTS `PointsPayment`;
DROP TABLE IF EXISTS `CosmeticItem`;
DROP TABLE IF EXISTS `CustomerCourse`;
DROP TABLE IF EXISTS `CourseModule`;
DROP TABLE IF EXISTS `Course`;
DROP TABLE IF EXISTS `Payment`;
DROP TABLE IF EXISTS `CustomerSubscription`;
DROP TABLE IF EXISTS `Subscription`;
DROP TABLE IF EXISTS `User`;

/* create tables */
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
CREATE TABLE Subscription (
      `SubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
      `Type` VARCHAR(50) NOT NULL,
      `PricePerCycle` DECIMAL(10,2) NOT NULL
);

CREATE TABLE CustomerSubscription (
      `CustomerSubscriptionID` VARCHAR(36) PRIMARY KEY NOT NULL,
      `StartDate` DATETIME NOT NULL,
      `EndDate` DATETIME NOT NULL,
      `Status` VARCHAR(50) NOT NULL,
      `AdriaID` VARCHAR(36) NOT NULL,
      `SubscriptionID` VARCHAR(36) NOT NULL,

      FOREIGN KEY (AdriaID) REFERENCES User(AdriaID),
      FOREIGN KEY (SubscriptionID) REFERENCES Subscription(SubscriptionID)
);

CREATE TABLE Payment (
     `TransactionID` VARCHAR(36) PRIMARY KEY NOT NULL,
     `AdriaCoinAmount` DECIMAL(10,2) NOT NULL,
     `TransactionDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `AdriaID` VARCHAR(36) NOT NULL,

     FOREIGN KEY (AdriaID) REFERENCES User(AdriaID)
);

-- COURSES

CREATE TABLE Course (
    `CourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `CourseName` VARCHAR(255) NOT NULL,
    `CourseDifficulty` VARCHAR(50) NOT NULL,
    `CourseDescription` TEXT,
    `PointsOnCompletion` INT NOT NULL,
    `EstimatedDurationInHours` INT,
    `CoverImageID` VARCHAR(255),
    `BannerImageID` VARCHAR(255),
    `SubscriptionRequired` VARCHAR(100) NOT NULL,

    FOREIGN KEY (SubscriptionRequired) REFERENCES Subscription(SubscriptionID)
);

CREATE TABLE CourseModule (
    `CourseModuleID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `ModuleName` VARCHAR(255) NOT NULL,
    `CourseID` VARCHAR(36) NOT NULL,

    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)
);

CREATE TABLE CustomerCourse (
    `CustomerCourseID` VARCHAR(36) PRIMARY KEY NOT NULL,
    `CompletionStatus` VARCHAR(50) NOT NULL,
    `ProgressPercentage` INT NOT NULL DEFAULT 0,
    `StartDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `CompletionDate` DATETIME,
    `AdriaID` VARCHAR(36) NOT NULL,
    `CourseID` VARCHAR(36) NOT NULL,

    FOREIGN KEY (AdriaID) REFERENCES User(AdriaID),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)
);

-- COSMETICS & POINTS
CREATE TABLE CosmeticItem (
  `CosmeticItemID` VARCHAR(36) PRIMARY KEY NOT NULL,
  `ItemName` VARCHAR(100) NOT NULL,
  `ItemType` VARCHAR(50) NOT NULL,
  `PointsCost` INT NOT NULL
);

CREATE TABLE PointsPayment (
   `PointsTransactionID` VARCHAR(36) PRIMARY KEY NOT NULL,
   `PointsAmount` INT NOT NULL,
   `TransactionDate` DATETIME NOT NULL DEFAULT GETDATE(),
   `AdriaID` VARCHAR(36) NOT NULL,
   `CosmeticItemID` VARCHAR(36) NOT NULL,

   FOREIGN KEY (AdriaID) REFERENCES User(AdriaID),
   FOREIGN KEY (CosmeticItemID) REFERENCES CosmeticItem(CosmeticItemID)
);