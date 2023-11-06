using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rentcarjwt.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ServerRcar");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cars",
                schema: "ServerRcar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<float>(type: "float", nullable: true),
                    brand = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    age = table.Column<int>(type: "int", nullable: true),
                    climate = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    door = table.Column<int>(type: "int", nullable: true),
                    color = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numberOfSeats = table.Column<int>(type: "int", nullable: false),
                    engineCapacity = table.Column<float>(type: "float", nullable: false),
                    transmissionType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fuelEype = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", nullable: true),
                    dateСreation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    deleteDt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarkerCars",
                schema: "ServerRcar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdCar = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    lat = table.Column<double>(type: "double", nullable: false),
                    lng = table.Column<double>(type: "double", nullable: false),
                    fromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    toDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkerCars", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "ServerRcar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    firstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    emailConfirmCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    age = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    drivingExperience = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passwordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegsterDt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeleteDt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "ServerRcar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    txt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserLessorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserTenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CarId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "ServerRcar",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserLessorId",
                        column: x => x.UserLessorId,
                        principalSchema: "ServerRcar",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserTenantId",
                        column: x => x.UserTenantId,
                        principalSchema: "ServerRcar",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReserveCars",
                schema: "ServerRcar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CarId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReserveCars_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "ServerRcar",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReserveCars_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ServerRcar",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CarId",
                schema: "ServerRcar",
                table: "Messages",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserLessorId",
                schema: "ServerRcar",
                table: "Messages",
                column: "UserLessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserTenantId",
                schema: "ServerRcar",
                table: "Messages",
                column: "UserTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveCars_CarId",
                schema: "ServerRcar",
                table: "ReserveCars",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveCars_UserId",
                schema: "ServerRcar",
                table: "ReserveCars",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkerCars",
                schema: "ServerRcar");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "ServerRcar");

            migrationBuilder.DropTable(
                name: "ReserveCars",
                schema: "ServerRcar");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "ServerRcar");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "ServerRcar");
        }
    }
}
