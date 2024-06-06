using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task11.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure the __EFMigrationsHistory table exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
                BEGIN
                    CREATE TABLE [__EFMigrationsHistory] (
                        [MigrationId] nvarchar(150) NOT NULL,
                        [ProductVersion] nvarchar(32) NOT NULL,
                        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
                    );
                END;
            ");

            // Check if the AnimalTypes table exists before creating it
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[AnimalTypes]') IS NULL
                BEGIN
                    CREATE TABLE [AnimalTypes] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NOT NULL,
                        CONSTRAINT [PK_AnimalTypes] PRIMARY KEY ([Id])
                    );
                END;
            ");

            // Check if the Employees table exists before creating it
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[Employees]') IS NULL
                BEGIN
                    CREATE TABLE [Employees] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NOT NULL,
                        [PhoneNumber] nvarchar(max) NOT NULL,
                        [Email] nvarchar(max) NOT NULL,
                        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
                    );
                END;
            ");

            // Check if the Animals table exists before creating it
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[Animals]') IS NULL
                BEGIN
                    CREATE TABLE [Animals] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NOT NULL,
                        [AnimalTypesId] int NOT NULL,
                        [RowVersion] rowversion NOT NULL,
                        CONSTRAINT [PK_Animals] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Animals_AnimalTypes_AnimalTypesId] FOREIGN KEY ([AnimalTypesId]) REFERENCES [AnimalTypes] ([Id]) ON DELETE CASCADE
                    );
                END;
            ");

            // Check if the Visits table exists before creating it
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[Visits]') IS NULL
                BEGIN
                    CREATE TABLE [Visits] (
                        [Id] int NOT NULL IDENTITY,
                        [EmployeeId] int NOT NULL,
                        [AnimalId] int NOT NULL,
                        [Date] datetime2 NOT NULL,
                        [RowVersion] rowversion NOT NULL,
                        CONSTRAINT [PK_Visits] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Visits_Animals_AnimalId] FOREIGN KEY ([AnimalId]) REFERENCES [Animals] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_Visits_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
                    );
                END;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_AnimalTypesId",
                table: "Animals",
                column: "AnimalTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_AnimalId",
                table: "Visits",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_EmployeeId",
                table: "Visits",
                column: "EmployeeId");

            // Insert a record into the __EFMigrationsHistory table
            migrationBuilder.Sql(@"
                INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                VALUES (N'20240606005942_InitialCreate', N'9.0.0-preview.4.24267.1');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "AnimalTypes");
        }
    }
}
