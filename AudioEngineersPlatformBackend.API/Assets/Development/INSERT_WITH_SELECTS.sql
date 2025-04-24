-- Role
INSERT INTO Role(RoleName)
VALUES ('Admin'),
       ('Audio Engineer'),
       ('Client');

SELECT *
FROM Role

-- SocialMediaTypeDict
INSERT INTO SocialMediaTypeDict(SocialMediaTypeName)
VALUES ('Facebook'),
       ('Instagram'),
       ('Linkedin'),
       ('Youtube');

SELECT *
FROM SocialMediaTypeDict

-- User
/*INSERT INTO [User](Username, Email, PhoneNumber, FirstName, LastName, Password, DateCreated, DateDeleted, IsDeleted,
                   IdRole)
VALUES ('TestUser', 'test@test.com', '+48696784867', 'John', 'Doe', 'HashedPassword', GETDATE(), null, 0,
        1);*/
SELECT *
FROM [User]

-- DELETE FROM [User]

-- UserLog
SELECT *
FROM UserLog;

-- DELETE FROM UserLog

