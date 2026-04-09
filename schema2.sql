CREATE DATABASE CapstoneDB;
GO
USE CapstoneDB;
GO


-- ============================================================
-- Users
-- =============================================================
CREATE TABLE Users (
    user_id         INT IDENTITY PRIMARY KEY,
    email           VARCHAR(255) NOT NULL UNIQUE,
    password_hash   VARCHAR(255) NOT NULL,
    user_name       VARCHAR(255) NOT NULL,
    created_at      DATETIME     NOT NULL DEFAULT GETDATE()
);

CREATE TABLE PasswordResetTokens (
    reset_token_id  INT IDENTITY PRIMARY KEY,
    user_id         INT NOT NULL,
    token_hash      VARCHAR(255) NOT NULL,
    expires_at      DATETIME NOT NULL,
    used_at         DATETIME NULL,
    created_at      DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_PasswordResetTokens_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- ============================================================
-- Brand
--   NULL user_id = system default (NVIDIA, Intel, …)
--   Non-NULL     = user-created custom brand
-- ============================================================
CREATE TABLE Brands (
    brand_id        INT IDENTITY PRIMARY KEY,
    user_id         INT NULL,
    name            VARCHAR(255) NOT NULL,
    created_at      DATETIME     NOT NULL DEFAULT GETDATE(),
    is_official     BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Brands_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- ============================================================
-- Category
--   Same system-default vs. user-created pattern as Brand.
-- ============================================================
CREATE TABLE Categories (
    category_id      INT IDENTITY PRIMARY KEY,
    user_id          INT NULL,
    name             VARCHAR(255) NOT NULL,
    description      VARCHAR(500) NULL,
    created_at       DATETIME NOT NULL DEFAULT GETDATE(),
    is_official      BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Categories_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- ============================================================
-- Item
--   Total participation in: owns (User), classifies (Category)
--   Partial participation in: identifies (Brand — nullable)
-- ============================================================
CREATE TABLE Items (
    item_id               INT IDENTITY PRIMARY KEY,
    user_id               INT NOT NULL,
    category_id           INT NOT NULL,
    brand_id              INT NULL,
    name                  VARCHAR(255) NOT NULL,
    model                 VARCHAR(255) NULL,
    purchase_date         DATE NULL,
    purchase_price        DECIMAL(10, 2) NULL,
    maybe_sell_threshold  DECIMAL(10, 2) NULL DEFAULT 50.00,  -- User-defined threshold for "maybe sell" alert
    original_value        DECIMAL(10, 2) NULL,  -- Optional field to store original value for depreciation tracking
    condition             VARCHAR(255) NULL DEFAULT 'Good',
    notes                 VARCHAR(1000) NULL,
    created_at            DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at            DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Items_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT FK_Items_Categories
        FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    CONSTRAINT FK_Items_Brands
        FOREIGN KEY (brand_id) REFERENCES Brands(brand_id)
);

-- ============================================================
-- WarrantyPolicy
--   Total participation in: scopes (Brand)
--   Partial participation in: scopes (Category — nullable,
--     allowing brand-wide defaults when CategoryId is NULL)
-- ============================================================
CREATE TABLE WarrantyPolicies (
    warranty_policy_id     INT IDENTITY PRIMARY KEY,
    brand_id               INT NOT NULL,
    category_id            INT NOT NULL,
    warranty_term_months   INT NOT NULL,
    source                 VARCHAR(255) NULL,
    created_at             DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_WarrantyPolicies_Brands
        FOREIGN KEY (brand_id) REFERENCES Brands(brand_id),
    CONSTRAINT FK_WarrantyPolicies_Categories
        FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    CONSTRAINT UQ_WarrantyPolicies_BrandCategory
        UNIQUE (brand_id, category_id)
);

-- ============================================================
-- ItemWarranty (weak entity — depends on Item)
--   Total participation in: has (Item — ItemId NOT NULL)
--   Partial participation in: applies to (WarrantyPolicy —
--     nullable, NULL when user enters warranty manually)
-- ============================================================
CREATE TABLE ItemWarranties (
    item_warranty_id      INT IDENTITY PRIMARY KEY,
    item_id               INT NOT NULL,
    warranty_policy_id    INT NULL,
    warranty_start_date   DATE NULL,
    warranty_end_date     DATE NULL,
    is_manual_entry       BIT NOT NULL DEFAULT 1,
    notes                 VARCHAR(1000) NULL,

    CONSTRAINT FK_ItemWarranties_Items
        FOREIGN KEY (item_id) REFERENCES Items(item_id)
            ON DELETE CASCADE,
    CONSTRAINT FK_ItemWarranties_WarrantyPolicies
        FOREIGN KEY (warranty_policy_id) REFERENCES WarrantyPolicies(warranty_policy_id),
    CONSTRAINT CK_ItemWarranties_Dates
        CHECK (warranty_end_date > warranty_start_date)
);

-- ============================================================
-- Valuation (weak entity — depends on Item)
--   Total participation in: valued by (Item — ItemId NOT NULL)
-- ============================================================
CREATE TABLE Valuations (
    valuation_id     INT IDENTITY PRIMARY KEY,
    item_id          INT NOT NULL,
    estimated_value  DECIMAL(10, 2) NULL,
    source           VARCHAR(255) NOT NULL DEFAULT 'manual',
    retrieved_at     DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Valuations_Items
        FOREIGN KEY (item_id) REFERENCES Items(item_id)
            ON DELETE CASCADE
);

-- ============================================================
-- WarrantyAlerts
--   (persistent until dismissed by user)
-- ============================================================
CREATE TABLE WarrantyAlerts (
    alert_id         INT IDENTITY PRIMARY KEY,
    user_id          INT NOT NULL,
    item_id          INT NOT NULL,
    alert_type       VARCHAR(255) NOT NULL DEFAULT 'active', -- "active", "expiring_soon", "expired"
    created_at       DATETIME NOT NULL DEFAULT GETDATE(),
    dismissed_at     DATETIME NULL,

    CONSTRAINT FK_WarrantyAlerts_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
            ON DELETE CASCADE,
    CONSTRAINT FK_WarrantyAlerts_Items
        FOREIGN KEY (item_id) REFERENCES Items(item_id)
            ON DELETE CASCADE
);