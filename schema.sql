CREATE DATABASE CapstoneDB;
GO
USE CapstoneDB;
GO

CREATE TABLE Users (
    user_email    VARCHAR(255) PRIMARY KEY,
    user_password VARCHAR(255) NOT NULL,
    user_name     VARCHAR(255) NOT NULL
);

CREATE TABLE HardwareCategories (
    category_id   INT IDENTITY PRIMARY KEY,
    category_name VARCHAR(255) NOT NULL,
    created_by    VARCHAR(255),            -- Email of the user who created this category
    is_official   BIT NOT NULL DEFAULT 0,  -- Indicates if this category is predefined
    FOREIGN KEY (created_by) REFERENCES Users(user_email)
);

CREATE TABLE HardwareModels (
    model_id         INT IDENTITY PRIMARY KEY,
    category_id      INT NOT NULL,
    model_brand      NVARCHAR(255) NOT NULL,
    model_name       NVARCHAR(255) NOT NULL,
    model_specs      NVARCHAR(MAX) NULL,       -- Optional JSON or text field for specifications
    is_official      BIT NOT NULL DEFAULT 0,   -- Indicates if this model is predefined
    created_by       VARCHAR(255) NOT NULL,    -- Email of the user who created this model
    FOREIGN KEY (category_id) REFERENCES HardwareCategories(category_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_email)
);

CREATE TABLE Items (
    item_id              INT IDENTITY PRIMARY KEY,
    user_email           VARCHAR(255) NOT NULL,
    hardware_model_id    INT NOT NULL,
    custom_brand         NVARCHAR(255) NULL,
    custom_name          NVARCHAR(255) NULL,
    custom_specs         NVARCHAR(MAX) NULL,
    purchase_date        DATE NULL,
    purchase_price       DECIMAL(18, 2) NULL,
    purchase_condition   VARCHAR(255) NULL,
    estimated_value      DECIMAL(18, 2) NULL,
    FOREIGN KEY (user_email) REFERENCES Users(user_email),
    FOREIGN KEY (hardware_model_id) REFERENCES HardwareModels(model_id)
);


CREATE TABLE WarrantyPolicies (
    warranty_id          INT IDENTITY PRIMARY KEY,
    brand                NVARCHAR(255) NOT NULL,
    category             VARCHAR(255) NOT NULL,
    default_term_months  INT NOT NULL,
    region               NVARCHAR(255) NOT NULL,
    notes                NVARCHAR(MAX) NULL
);

CREATE TABLE ItemWarranties (
    item_warranty_id      INT IDENTITY PRIMARY KEY,
    item_id               INT NOT NULL,
    policy_id             INT NULL,
    warranty_start        DATE NULL,
    explicit_term_months  INT NULL,
    explicit_end_date     DATE NULL,
    notes                 NVARCHAR(MAX) NULL,
    FOREIGN KEY (item_id) REFERENCES Items(item_id),
    FOREIGN KEY (policy_id) REFERENCES WarrantyPolicies(warranty_id),
    CHECK (
        (policy_id IS NOT NULL AND explicit_term_months IS NULL AND explicit_end_date IS NULL) OR
        (policy_id IS NULL AND explicit_term_months IS NOT NULL AND explicit_end_date IS NOT NULL)
    )
);