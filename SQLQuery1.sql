/*איך עובד ה APIKEY*/
/* 
בכל פעם שהתוסף שולח קוד לתיקון, הוא מצרף בראש הבקשה (בתוך ה-Headers) את ה-ApiKey שלו.

כשה-Web API שלך מקבל את הבקשה, הוא עושה את הפעולות הבאות:

הוא לוקח את ה-ApiKey שהגיע מהתוסף.

הוא הולך לטבלת Users ב-SQL Server ומחפש: "למי מהמשתמשים שלי יש את ה-ApiKey הזה?".

אם הוא מצא משתמש תואם: השרת מבין מיד מי המשתמש (למשל, משתמש מספר 1), יודע האם מגיע לו שירות (לפי ה-IsPremium), ומאשר את הקריאה ל-AI.

אם המפתח לא קיים ב-DB: השרת חוסם את הבקשה ומחזיר שגיאת אבטחה (401 Unauthorized).
*/

Create TABLE Users (
    -- Id הוא מספר רץ, מתחיל ב-1 וקופץ ב-1, והוא המפתח הראשי של הטבלה
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    
    -- כתובת המייל של המשתמש, מקסימום 100 תווים, חייב להכיל ערך
    Email NVARCHAR(100) NOT NULL,
    
    -- ה-Token המאובטח של התוסף, יכול להכיל מחרוזת ארוכה מאוד, חייב להכיל ערך
    ApiKey NVARCHAR(MAX) NOT NULL,
    
    -- שדה שמסמן האם המשתמש שילם (אמת/שקר), ברירת המחדל היא שלא (0 = False)
    IsPremium BIT NOT NULL DEFAULT 0,
    
    -- תאריך ושעת ההרשמה של המשתמש, ברירת המחדל היא הזמן הנוכחי של השרת
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

Create TABLE RefactorLogs (
    -- Id הוא מספר רץ ייחודי של הטבלה הזו (הלוגים), מתחיל ב-1 וקופץ ב-1
    RefactorLogId INT IDENTITY(1,1) PRIMARY KEY,
    
    -- קישור לטבלת USERS- FK
    UserId INT NOT NULL,
    
    -- הקוד המקורי שהמפתח שלח, יכול להכיל טקסט ארוך מאוד
    OriginalCode NVARCHAR(MAX) NOT NULL,
    
    -- הקוד המתוקן שחזר מה-AI. השדה הזה הוא NULL כי אם הבקשה נכשלה, הוא יישאר ריק
    CorrectedCode NVARCHAR(MAX) NULL,
    
    -- סוג הפעולה (למשל: 'ShortenCode', 'FixTypos', 'RenameVariables')
    ActionType VARCHAR(50) NOT NULL,
    
    -- תאריך ושעת ביצוע התיקון, ברירת המחדל היא הזמן הנוכחי
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    
    -- הגדרת המפתח הזר שמקשר את ה-UserId לעמודת Id בטבלת Users
    CONSTRAINT FK_RefactorLogs_Users FOREIGN KEY (UserId) 
    REFERENCES Users(UserId)
);

    /* IsPremium= 0 = משתמש חינמי, IsPremium= 0 = שילם על פרמיום*/
	/*  ApiKey= סיסמה סודית והייחודית של התוסף, כל פעם שהמשתמש רוצה להשתמש בתוסף, המערכת יוצרת לו סיסמה, המשתמש מעתיק את זה פעם אחת להגדרות של התוסף*/
    INSERT INTO Users (Email, ApiKey, IsPremium)
    VALUES ('Ms23hh%1@gmail.com', 'PROXER_SUPER_SECRET_KEY_123', 0);

	SELECT * FROM RefactorLogs;