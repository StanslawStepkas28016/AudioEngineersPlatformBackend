-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-03-27 12:15:43.78

-- tables
-- Table: Advert
CREATE TABLE Advert (
                        IdAdvert int  NOT NULL IDENTITY,
                        Title varchar(150)  NOT NULL,
                        Description varchar(500)  NOT NULL,
                        Price money  NOT NULL,
                        IdUser int  NOT NULL,
                        IdAdvertLog int  NOT NULL,
                        CONSTRAINT Advert_pk PRIMARY KEY  (IdAdvert)
);

-- Table: AdvertCategories
CREATE TABLE AdvertCategories (
                                  IdAdvert int  NOT NULL,
                                  IdAdvertCategoryDict int  NOT NULL,
                                  CONSTRAINT AdvertCategories_pk PRIMARY KEY  (IdAdvert,IdAdvertCategoryDict)
);

-- Table: AdvertCategoryDict
CREATE TABLE AdvertCategoryDict (
                                    IdAdvertCategoryDict int  NOT NULL IDENTITY,
                                    CategoryName varchar(250)  NOT NULL,
                                    CONSTRAINT AdvertCategoryDict_pk PRIMARY KEY  (IdAdvertCategoryDict)
);

-- Table: AdvertLog
CREATE TABLE AdvertLog (
                           IdAdvertLog int  NOT NULL IDENTITY,
                           DateCreated datetime  NOT NULL,
                           DateDeleted datetime  NULL,
                           DateModified datetime  NULL,
                           IsActive bit  NOT NULL,
                           IsDeleted bit  NOT NULL,
                           IdUser_Modifier int  NOT NULL,
                           CONSTRAINT AdvertLog_pk PRIMARY KEY  (IdAdvertLog)
);

-- Table: Image
CREATE TABLE Image (
                       IdImage int  NOT NULL IDENTITY,
                       ImageLink varchar(500)  NOT NULL,
                       IdAdvert int  NOT NULL,
                       CONSTRAINT Image_pk PRIMARY KEY  (IdImage)
);

-- Table: PlaylistLink
CREATE TABLE PlaylistLink (
                              IdPlaylistLink int  NOT NULL IDENTITY,
                              Link varchar(250)  NOT NULL,
                              IdAdvert int  NOT NULL,
                              IdPlaylistType int  NOT NULL,
                              CONSTRAINT PlaylistLink_pk PRIMARY KEY  (IdPlaylistLink)
);

-- Table: PlaylistTypeDict
CREATE TABLE PlaylistTypeDict (
                                  IdPlaylistTypeDict int  NOT NULL IDENTITY,
                                  PlaylistTypeName varchar(250)  NOT NULL,
                                  CONSTRAINT PlaylistTypeDict_pk PRIMARY KEY  (IdPlaylistTypeDict)
);

-- Table: Review
CREATE TABLE Review (
                        IdReview int  NOT NULL IDENTITY,
                        Content varchar(500)  NOT NULL,
                        SatisfactionLevel tinyint  NOT NULL,
                        IdUser_Reviewer int  NOT NULL,
                        IdAdvert int  NOT NULL,
                        IdReviewLog int  NOT NULL,
                        CONSTRAINT Review_pk PRIMARY KEY  (IdReview)
);

-- Table: ReviewLog
CREATE TABLE ReviewLog (
                           IdReviewLog int  NOT NULL IDENTITY,
                           DateCreated datetime  NOT NULL,
                           DateDeleted datetime  NOT NULL,
                           IsDeleted bit  NOT NULL,
                           CONSTRAINT ReviewLog_pk PRIMARY KEY  (IdReviewLog)
);

-- Table: Role
CREATE TABLE Role (
                      IdRole int  NOT NULL IDENTITY,
                      RoleName varchar(20)  NOT NULL,
                      CONSTRAINT Role_pk PRIMARY KEY  (IdRole)
);

-- Table: SocialMediaLink
CREATE TABLE SocialMediaLink (
                                 IdSocialMediaLink int  NOT NULL IDENTITY,
                                 SocialMediaLink varchar(250)  NOT NULL,
                                 IdUser int  NOT NULL,
                                 IdSocialMediaTypeDict int  NOT NULL,
                                 CONSTRAINT SocialMediaLink_pk PRIMARY KEY  (IdSocialMediaLink)
);

-- Table: SocialMediaTypeDict
CREATE TABLE SocialMediaTypeDict (
                                     IdSocialMediaType int  NOT NULL IDENTITY,
                                     SocialMediaTypeName varchar(250)  NOT NULL,
                                     CONSTRAINT SocialMediaTypeDict_pk PRIMARY KEY  (IdSocialMediaType)
);

-- Table: User
CREATE TABLE "User" (
                        IdUser int  NOT NULL IDENTITY,
                        FirstName varchar(50)  NOT NULL,
                        LastName varchar(50)  NOT NULL,
                        Username varchar(50)  NOT NULL,
                        Email varchar(30)  NOT NULL,
                        PhoneNumber varchar(16)  NOT NULL,
                        Password varchar(256)  NOT NULL,
                        IdRole int  NOT NULL,
                        IdUserLog int  NOT NULL,
                        CONSTRAINT User_pk PRIMARY KEY  (IdUser)
);

-- Table: UserLog
CREATE TABLE UserLog (
                         IdUserLog int  NOT NULL IDENTITY,
                         DateCreated datetime  NOT NULL,
                         DateDeleted datetime  NULL,
                         IsDeleted bit  NOT NULL,
                         VerificationCode varchar(6)  NULL,
                         VerificationCodeExpiration datetime  NULL,
                         IsVerified bit  NOT NULL,
                         DateLastLogin datetime NULL,
                         CONSTRAINT UserLog_pk PRIMARY KEY  (IdUserLog)
);

-- foreign keys
-- Reference: AdvertLog_User (table: AdvertLog)
ALTER TABLE AdvertLog ADD CONSTRAINT AdvertLog_User
    FOREIGN KEY (IdUser_Modifier)
        REFERENCES "User" (IdUser);

-- Reference: Advert_AdvertLog (table: Advert)
ALTER TABLE Advert ADD CONSTRAINT Advert_AdvertLog
    FOREIGN KEY (IdAdvertLog)
        REFERENCES AdvertLog (IdAdvertLog);

-- Reference: Advert_User (table: Advert)
ALTER TABLE Advert ADD CONSTRAINT Advert_User
    FOREIGN KEY (IdUser)
        REFERENCES "User" (IdUser);

-- Reference: Image_Advert (table: Image)
ALTER TABLE Image ADD CONSTRAINT Image_Advert
    FOREIGN KEY (IdAdvert)
        REFERENCES Advert (IdAdvert);

-- Reference: PlaylistLink_Advert (table: PlaylistLink)
ALTER TABLE PlaylistLink ADD CONSTRAINT PlaylistLink_Advert
    FOREIGN KEY (IdAdvert)
        REFERENCES Advert (IdAdvert);

-- Reference: PlaylistLink_PlaylistTypeDict (table: PlaylistLink)
ALTER TABLE PlaylistLink ADD CONSTRAINT PlaylistLink_PlaylistTypeDict
    FOREIGN KEY (IdPlaylistType)
        REFERENCES PlaylistTypeDict (IdPlaylistTypeDict);

-- Reference: Review_Advert (table: Review)
ALTER TABLE Review ADD CONSTRAINT Review_Advert
    FOREIGN KEY (IdAdvert)
        REFERENCES Advert (IdAdvert);

-- Reference: Review_ReviewLog (table: Review)
ALTER TABLE Review ADD CONSTRAINT Review_ReviewLog
    FOREIGN KEY (IdReviewLog)
        REFERENCES ReviewLog (IdReviewLog);

-- Reference: Review_User (table: Review)
ALTER TABLE Review ADD CONSTRAINT Review_User
    FOREIGN KEY (IdUser_Reviewer)
        REFERENCES "User" (IdUser);

-- Reference: SocialMediaLink_SocialMediaTypeDict (table: SocialMediaLink)
ALTER TABLE SocialMediaLink ADD CONSTRAINT SocialMediaLink_SocialMediaTypeDict
    FOREIGN KEY (IdSocialMediaTypeDict)
        REFERENCES SocialMediaTypeDict (IdSocialMediaType);

-- Reference: SocialMediaLink_User (table: SocialMediaLink)
ALTER TABLE SocialMediaLink ADD CONSTRAINT SocialMediaLink_User
    FOREIGN KEY (IdUser)
        REFERENCES "User" (IdUser);

-- Reference: Table_10_Advert (table: AdvertCategories)
ALTER TABLE AdvertCategories ADD CONSTRAINT Table_10_Advert
    FOREIGN KEY (IdAdvert)
        REFERENCES Advert (IdAdvert);

-- Reference: Table_10_AdvertCategoryDict (table: AdvertCategories)
ALTER TABLE AdvertCategories ADD CONSTRAINT Table_10_AdvertCategoryDict
    FOREIGN KEY (IdAdvertCategoryDict)
        REFERENCES AdvertCategoryDict (IdAdvertCategoryDict);

-- Reference: User_Role (table: User)
ALTER TABLE "User" ADD CONSTRAINT User_Role
    FOREIGN KEY (IdRole)
        REFERENCES Role (IdRole);

-- Reference: User_UserLog (table: User)
ALTER TABLE "User" ADD CONSTRAINT User_UserLog
    FOREIGN KEY (IdUserLog)
        REFERENCES UserLog (IdUserLog);

-- End of file.

