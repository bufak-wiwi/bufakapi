-- phpMyAdmin SQL Dump
-- version 4.6.6deb4
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 31, 2019 at 10:07 AM
-- Server version: 10.3.15-MariaDB-1
-- PHP Version: 7.3.4-2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bufakapiv2`
--

-- --------------------------------------------------------

--
-- Table structure for table `administrator`
--

CREATE TABLE `administrator` (
  `UID` varchar(255) NOT NULL,
  `ConferenceID` int(11) NOT NULL,
  `ValidUntil` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `applicationauth`
--

CREATE TABLE `applicationauth` (
  `ID` int(11) NOT NULL,
  `Conference_ID` int(11) NOT NULL,
  `Council_ID` int(11) NOT NULL,
  `Priority` int(11) NOT NULL,
  `Password` longtext DEFAULT NULL,
  `Used` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `auth`
--

CREATE TABLE `auth` (
  `TokenID` int(11) NOT NULL,
  `ApiKey` longtext DEFAULT NULL,
  `Note` longtext DEFAULT NULL,
  `CreatedOn` longtext DEFAULT NULL,
  `ValidUntil` longtext DEFAULT NULL,
  `ConferenceID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `conference`
--

CREATE TABLE `conference` (
  `ConferenceID` int(11) NOT NULL,
  `DateStart` longtext DEFAULT NULL,
  `DateEnd` longtext DEFAULT NULL,
  `CouncilID` int(11) NOT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0',
  `ConferenceApplicationPhase` bit(1) NOT NULL DEFAULT b'0',
  `WorkshopApplicationPhase` bit(1) NOT NULL DEFAULT b'0',
  `WorkshopSuggestionPhase` bit(1) NOT NULL DEFAULT b'0',
  `AttendeeCost` longtext DEFAULT NULL,
  `AlumnusCost` longtext DEFAULT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `conference_application`
--

CREATE TABLE `conference_application` (
  `ConferenceID` int(11) NOT NULL,
  `ApplicantUID` varchar(255) NOT NULL,
  `UserUID` varchar(255) DEFAULT NULL,
  `SensibleID` int(11) NOT NULL,
  `Priority` int(11) NOT NULL,
  `IsAlumnus` bit(1) NOT NULL,
  `IsBuFaKCouncil` bit(1) NOT NULL,
  `Note` longtext DEFAULT NULL,
  `Timestamp` longtext DEFAULT NULL,
  `IsHelper` bit(1) NOT NULL,
  `Hotel` longtext DEFAULT NULL,
  `Room` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `council`
--

CREATE TABLE `council` (
  `CouncilID` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `NameShort` longtext DEFAULT NULL,
  `City` longtext DEFAULT NULL,
  `State` longtext DEFAULT NULL,
  `University` longtext DEFAULT NULL,
  `UniversityShort` longtext DEFAULT NULL,
  `Address` longtext DEFAULT NULL,
  `ContactEmail` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `history`
--

CREATE TABLE `history` (
  `HistoryID` int(11) NOT NULL,
  `OldValue` longtext DEFAULT NULL,
  `ResponsibleUID` longtext DEFAULT NULL,
  `UserUID` varchar(255) DEFAULT NULL,
  `HistoryType` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `newsletter`
--

CREATE TABLE `newsletter` (
  `ID` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `Surname` longtext DEFAULT NULL,
  `Email` longtext DEFAULT NULL,
  `Studyplace` longtext DEFAULT NULL,
  `Sex` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `sensible`
--

CREATE TABLE `sensible` (
  `SensibleID` int(11) NOT NULL,
  `ConferenceID` int(11) NOT NULL,
  `Timestamp` longtext DEFAULT NULL,
  `BuFaKCount` int(11) NOT NULL,
  `UID` longtext DEFAULT NULL,
  `EatingPreferences` longtext DEFAULT NULL,
  `Intolerances` longtext DEFAULT NULL,
  `SleepingPreferences` longtext DEFAULT NULL,
  `Telephone` longtext DEFAULT NULL,
  `ExtraNote` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `UID` varchar(255) NOT NULL,
  `Name` longtext DEFAULT 'NULL',
  `Surname` longtext DEFAULT NULL,
  `Birthday` longtext DEFAULT NULL,
  `Email` longtext DEFAULT NULL,
  `CouncilID` int(11) NOT NULL,
  `Address` longtext DEFAULT NULL,
  `Sex` longtext DEFAULT NULL,
  `Note` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0',
  `IsSuperAdmin` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `workshop`
--

CREATE TABLE `workshop` (
  `WorkshopID` int(11) NOT NULL,
  `ConferenceID` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `NameShort` longtext DEFAULT NULL,
  `Overview` longtext DEFAULT NULL,
  `MaxVisitors` int(11) NOT NULL,
  `Difficulty` longtext DEFAULT NULL,
  `HostUID` longtext DEFAULT NULL,
  `HostName` longtext DEFAULT NULL,
  `UserUID` varchar(255) DEFAULT NULL,
  `Place` longtext DEFAULT NULL,
  `Start` longtext DEFAULT NULL,
  `Duration` int(11) NOT NULL,
  `MaterialNote` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `workshop_application`
--

CREATE TABLE `workshop_application` (
  `WorkshopID` int(11) NOT NULL,
  `ApplicantUID` varchar(255) NOT NULL,
  `IsHelper` bit(1) NOT NULL,
  `UserUID` varchar(255) DEFAULT NULL,
  `Priority` int(11) NOT NULL,
  `Status` longtext DEFAULT NULL,
  `Invalid` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `administrator`
--
ALTER TABLE `administrator`
  ADD PRIMARY KEY (`UID`,`ConferenceID`),
  ADD KEY `IX_Administrator_ConferenceID` (`ConferenceID`);

--
-- Indexes for table `applicationauth`
--
ALTER TABLE `applicationauth`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `auth`
--
ALTER TABLE `auth`
  ADD PRIMARY KEY (`TokenID`);

--
-- Indexes for table `conference`
--
ALTER TABLE `conference`
  ADD PRIMARY KEY (`ConferenceID`);

--
-- Indexes for table `conference_application`
--
ALTER TABLE `conference_application`
  ADD PRIMARY KEY (`ConferenceID`,`ApplicantUID`),
  ADD KEY `IX_Conference_Application_SensibleID` (`SensibleID`),
  ADD KEY `IX_Conference_Application_UserUID` (`UserUID`);

--
-- Indexes for table `council`
--
ALTER TABLE `council`
  ADD PRIMARY KEY (`CouncilID`);

--
-- Indexes for table `history`
--
ALTER TABLE `history`
  ADD PRIMARY KEY (`HistoryID`),
  ADD KEY `IX_History_UserUID` (`UserUID`);

--
-- Indexes for table `newsletter`
--
ALTER TABLE `newsletter`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `sensible`
--
ALTER TABLE `sensible`
  ADD PRIMARY KEY (`SensibleID`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UID`);

--
-- Indexes for table `workshop`
--
ALTER TABLE `workshop`
  ADD PRIMARY KEY (`WorkshopID`),
  ADD KEY `IX_Workshop_ConferenceID` (`ConferenceID`),
  ADD KEY `IX_Workshop_UserUID` (`UserUID`);

--
-- Indexes for table `workshop_application`
--
ALTER TABLE `workshop_application`
  ADD PRIMARY KEY (`WorkshopID`,`ApplicantUID`),
  ADD KEY `IX_Workshop_Application_UserUID` (`UserUID`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `applicationauth`
--
ALTER TABLE `applicationauth`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2588;
--
-- AUTO_INCREMENT for table `auth`
--
ALTER TABLE `auth`
  MODIFY `TokenID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `conference`
--
ALTER TABLE `conference`
  MODIFY `ConferenceID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `council`
--
ALTER TABLE `council`
  MODIFY `CouncilID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=213;
--
-- AUTO_INCREMENT for table `history`
--
ALTER TABLE `history`
  MODIFY `HistoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=290;
--
-- AUTO_INCREMENT for table `newsletter`
--
ALTER TABLE `newsletter`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=132;
--
-- AUTO_INCREMENT for table `sensible`
--
ALTER TABLE `sensible`
  MODIFY `SensibleID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=352;
--
-- AUTO_INCREMENT for table `workshop`
--
ALTER TABLE `workshop`
  MODIFY `WorkshopID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=336;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `administrator`
--
ALTER TABLE `administrator`
  ADD CONSTRAINT `FK_Administrator_Conference_ConferenceID` FOREIGN KEY (`ConferenceID`) REFERENCES `conference` (`ConferenceID`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Administrator_User_UID` FOREIGN KEY (`UID`) REFERENCES `user` (`UID`) ON DELETE CASCADE;

--
-- Constraints for table `conference_application`
--
ALTER TABLE `conference_application`
  ADD CONSTRAINT `FK_Conference_Application_Conference_ConferenceID` FOREIGN KEY (`ConferenceID`) REFERENCES `conference` (`ConferenceID`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Conference_Application_Sensible_SensibleID` FOREIGN KEY (`SensibleID`) REFERENCES `sensible` (`SensibleID`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Conference_Application_User_UserUID` FOREIGN KEY (`UserUID`) REFERENCES `user` (`UID`);

--
-- Constraints for table `history`
--
ALTER TABLE `history`
  ADD CONSTRAINT `FK_History_User_UserUID` FOREIGN KEY (`UserUID`) REFERENCES `user` (`UID`);

--
-- Constraints for table `workshop`
--
ALTER TABLE `workshop`
  ADD CONSTRAINT `FK_Workshop_Conference_ConferenceID` FOREIGN KEY (`ConferenceID`) REFERENCES `conference` (`ConferenceID`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Workshop_User_UserUID` FOREIGN KEY (`UserUID`) REFERENCES `user` (`UID`);

--
-- Constraints for table `workshop_application`
--
ALTER TABLE `workshop_application`
  ADD CONSTRAINT `FK_Workshop_Application_User_UserUID` FOREIGN KEY (`UserUID`) REFERENCES `user` (`UID`),
  ADD CONSTRAINT `FK_Workshop_Application_Workshop_WorkshopID` FOREIGN KEY (`WorkshopID`) REFERENCES `workshop` (`WorkshopID`) ON DELETE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
