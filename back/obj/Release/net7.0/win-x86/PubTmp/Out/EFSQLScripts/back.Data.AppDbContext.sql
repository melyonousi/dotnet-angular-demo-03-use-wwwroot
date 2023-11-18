CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "BlogPosts" (
        "Id" uuid NOT NULL,
        "Title" text NOT NULL,
        "ShortDescription" text NULL,
        "Content" text NULL,
        "FeaturedImageUrl" text NOT NULL,
        "UrlHandle" text NULL,
        "PublishedDate" timestamp with time zone NOT NULL,
        "Author" text NOT NULL,
        "IsVisible" boolean NOT NULL,
        CONSTRAINT "PK_BlogPosts" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "Categories" (
        "Id" uuid NOT NULL,
        "Name" text NOT NULL,
        "UrlHandle" text NULL,
        CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "Products" (
        "Id" uuid NOT NULL,
        "Title" text NOT NULL,
        "Description" text NULL,
        "Thumbnail" text NOT NULL,
        "PublishedDate" timestamp with time zone NOT NULL,
        "Visibility" boolean NOT NULL,
        CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "RefreshTokens" (
        "Id" uuid NOT NULL,
        "RToken" text NOT NULL,
        "Status" boolean NOT NULL,
        "UserId" uuid NOT NULL,
        CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "Roles" (
        "Id" uuid NOT NULL,
        "RoleKey" text NOT NULL,
        "RoleName" text NOT NULL,
        CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Name" text NOT NULL,
        "Email" text NOT NULL,
        "Password" text NOT NULL,
        "Credentials" text NOT NULL,
        "Token" text NULL,
        "Avatar" text NULL,
        "RoleId" uuid NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20231018205748_InitialMigrations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20231018205748_InitialMigrations', '7.0.12');
    END IF;
END $EF$;
COMMIT;

