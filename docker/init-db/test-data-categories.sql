-- ============================================
-- Test Data for Categories - Hierarchical Structure
-- Fixed: No CreatedDate, CreatedBy; Use CategoryDesciption (with typo)
-- ============================================
USE FUNewsManagementSystem;
GO

PRINT '=== Starting Category Test Data Creation ===';

-- ============================================
-- LEVEL 1: Root Categories
-- ============================================
PRINT 'Creating Level 1: Root Categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, IsActive)
VALUES 
    ('Technology', 'All about technology, innovations, and digital transformation', 1),
    ('Business', 'Business news, finance, startups, and economic trends', 1),
    ('Sports', 'Sports news, updates, and events from around the world', 1),
    ('Entertainment', 'Movies, music, celebrities, and pop culture', 1),
    ('Health', 'Health tips, medical news, and wellness advice', 1);

DECLARE @TechID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Technology');
DECLARE @BusinessID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Business');
DECLARE @SportID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Sports');
DECLARE @EntertainmentID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Entertainment');
DECLARE @HealthID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Health');

PRINT CONCAT('  - Technology ID: ', @TechID);
PRINT CONCAT('  - Business ID: ', @BusinessID);
PRINT CONCAT('  - Sports ID: ', @SportID);
PRINT CONCAT('  - Entertainment ID: ', @EntertainmentID);
PRINT CONCAT('  - Health ID: ', @HealthID);

-- ============================================
-- LEVEL 2: Sub-categories of Technology
-- ============================================
PRINT '';
PRINT 'Creating Level 2: Technology Sub-categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Artificial Intelligence', 'AI, Machine Learning, Deep Learning, and Neural Networks', @TechID, 1),
    ('Web Development', 'Frontend, Backend, Full-stack development technologies', @TechID, 1),
    ('Mobile Apps', 'iOS, Android, and cross-platform mobile development', @TechID, 1),
    ('Cybersecurity', 'Security best practices, threats, and data protection', @TechID, 1),
    ('Cloud Computing', 'AWS, Azure, GCP, and cloud infrastructure', @TechID, 1),
    ('DevOps', 'CI/CD, Docker, Kubernetes, automation tools', @TechID, 1);

-- ============================================
-- LEVEL 2: Sub-categories of Business
-- ============================================
PRINT 'Creating Level 2: Business Sub-categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Startups', 'Startup ecosystem, funding rounds, and venture capital', @BusinessID, 1),
    ('Stock Market', 'Stock trading, investing strategies, and market analysis', @BusinessID, 1),
    ('Cryptocurrency', 'Bitcoin, Ethereum, blockchain, and crypto trading', @BusinessID, 1),
    ('E-commerce', 'Online retail, marketplaces, and shopping trends', @BusinessID, 1),
    ('Real Estate', 'Property market, investment, and housing trends', @BusinessID, 1);

-- ============================================
-- LEVEL 2: Sub-categories of Sports
-- ============================================
PRINT 'Creating Level 2: Sports Sub-categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Football', 'Soccer news, leagues, and international tournaments', @SportID, 1),
    ('Basketball', 'NBA, international basketball, and college hoops', @SportID, 1),
    ('Tennis', 'Grand Slam tournaments, ATP, and WTA tours', @SportID, 1),
    ('Formula 1', 'F1 racing, drivers, teams, and championship standings', @SportID, 1),
    ('Olympics', 'Olympic games, athletes, and medal standings', @SportID, 1);

-- ============================================
-- LEVEL 2: Sub-categories of Entertainment
-- ============================================
PRINT 'Creating Level 2: Entertainment Sub-categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Movies', 'Hollywood, box office, film reviews, and trailers', @EntertainmentID, 1),
    ('TV Shows', 'Series, streaming platforms, and episode reviews', @EntertainmentID, 1),
    ('Music', 'Albums, concerts, music charts, and artist news', @EntertainmentID, 1),
    ('Gaming', 'Video games, esports, game reviews, and industry news', @EntertainmentID, 1);

-- ============================================
-- LEVEL 2: Sub-categories of Health
-- ============================================
PRINT 'Creating Level 2: Health Sub-categories...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Nutrition', 'Diet tips, healthy eating, and meal planning', @HealthID, 1),
    ('Fitness', 'Workout routines, exercise tips, and gym advice', @HealthID, 1),
    ('Mental Health', 'Psychology, stress management, and wellness', @HealthID, 1),
    ('Medical Research', 'Latest medical studies and healthcare innovations', @HealthID, 1);

-- ============================================
-- LEVEL 3: Sub-sub-categories (AI children)
-- ============================================
PRINT '';
PRINT 'Creating Level 3: AI Sub-categories...';

DECLARE @AIID SMALLINT = (SELECT CategoryID FROM Category WHERE CategoryName = 'Artificial Intelligence');

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Natural Language Processing', 'NLP, chatbots, language models, and text analysis', @AIID, 1),
    ('Computer Vision', 'Image recognition, object detection, and visual AI', @AIID, 1),
    ('Robotics', 'AI-powered robots, automation, and intelligent systems', @AIID, 1);

-- ============================================
-- Inactive Categories (for testing IsActive filter)
-- ============================================
PRINT '';
PRINT 'Creating Inactive Categories for testing...';

INSERT INTO Category (CategoryName, CategoryDesciption, ParentCategoryID, IsActive)
VALUES 
    ('Archived Tech', 'Old technology news - archived', @TechID, 0),
    ('Deprecated Category', 'This category is no longer active', NULL, 0);

-- ============================================
-- Summary
-- ============================================
PRINT '';
PRINT '=== Category Test Data Summary ===';
SELECT 
    'Total Categories' AS Metric,
    COUNT(*) AS Count
FROM Category
UNION ALL
SELECT 
    'Active Categories',
    COUNT(*)
FROM Category
WHERE IsActive = 1
UNION ALL
SELECT 
    'Inactive Categories',
    COUNT(*)
FROM Category
WHERE IsActive = 0
UNION ALL
SELECT 
    'Root Categories (Level 1)',
    COUNT(*)
FROM Category
WHERE ParentCategoryID IS NULL
UNION ALL
SELECT 
    'Level 2 Categories',
    COUNT(*)
FROM Category c1
WHERE ParentCategoryID IS NOT NULL 
  AND ParentCategoryID IN (SELECT CategoryID FROM Category WHERE ParentCategoryID IS NULL);

PRINT '';
PRINT '=== Hierarchical Structure Sample ===';
SELECT TOP 20
    c1.CategoryID AS ID,
    c1.CategoryName AS Name,
    c2.CategoryName AS ParentName,
    c1.IsActive AS Active,
    (SELECT COUNT(*) FROM Category WHERE ParentCategoryID = c1.CategoryID) AS Children
FROM Category c1
LEFT JOIN Category c2 ON c1.ParentCategoryID = c2.CategoryID
ORDER BY 
    CASE WHEN c1.ParentCategoryID IS NULL THEN 0 ELSE 1 END,
    c1.ParentCategoryID,
    c1.CategoryName;

PRINT '';
PRINT '=== Test Data Creation Complete! ===';
GO
