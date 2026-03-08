CREATE DATABASE CapstoneDB;
GO
USE CapstoneDB;
GO

CREATE TABLE Users (
    user_email    VARCHAR(255) PRIMARY KEY,
    user_password VARCHAR(255) NOT NULL,
    user_name     VARCHAR(255) NOT NULL
);

CREATE TABLE HardwareModels (
    model_id         INT IDENTITY PRIMARY KEY,
    model_category   VARCHAR(255) NOT NULL,
    model_brand      NVARCHAR(255) NOT NULL,
    model_name       NVARCHAR(255) NOT NULL,
    model_specs      NVARCHAR(MAX) NULL,     -- Optional JSON or text field for specifications
    UNIQUE (model_category, model_brand, model_name)
)

CREATE TABLE Items (
    item_id           INT IDENTITY PRIMARY KEY,
    user_email        VARCHAR(255) NOT NULL,
    hardware_model_id INT NOT NULL,
    custom_brand      NVARCHAR(255) NULL,
    custom_name       NVARCHAR(255) NULL,
    custom_specs      NVARCHAR(MAX) NULL,
    FOREIGN KEY (user_email) REFERENCES Users(user_email),
    FOREIGN KEY (hardware_model_id) REFERENCES HardwareModels(model_id)
)

CREATE TABLE WarrantyPolicies (
    warranty_id         INT IDENTITY PRIMARY KEY,
    brand               NVARCHAR(255) NOT NULL,
    category            VARCHAR(255) NOT NULL,
    default_term_months INT NOT NULL,
    region              NVARCHAR(255) NOT NULL,
    notes               NVARCHAR(MAX) NULL
);

ALTER TABLE HardwareModels
ADD warranty_id INT NULL,
FOREIGN KEY (warranty_id) REFERENCES WarrantyPolicies(warranty_id);