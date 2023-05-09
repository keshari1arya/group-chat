using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GroupChat.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUserXREF",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUserXREF", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupUserXREF_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUserXREF_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupMessageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageLikes_GroupMessages_GroupMessageId",
                        column: x => x.GroupMessageId,
                        principalTable: "GroupMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "CreatorId", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Description 1", false, "Group 1" },
                    { 2, 8, "Description 2", false, "Group 2" },
                    { 3, 5, "Description 3", false, "Group 3" },
                    { 4, 10, "Description 4", false, "Group 4" },
                    { 5, 1, "Description 5", false, "Group 5" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "Name", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "user1@mail.com", false, "User 1", "password1", "user1" },
                    { 2, "user2@mail.com", false, "User 2", "password2", "user2" },
                    { 3, "user3@mail.com", false, "User 3", "password3", "user3" },
                    { 4, "user4@mail.com", false, "User 4", "password4", "user4" },
                    { 5, "user5@mail.com", false, "User 5", "password5", "user5" },
                    { 6, "user6@mail.com", false, "User 6", "password6", "user6" },
                    { 7, "user7@mail.com", false, "User 7", "password7", "user7" },
                    { 8, "user8@mail.com", false, "User 8", "password8", "user8" },
                    { 9, "user9@mail.com", false, "User 9", "password9", "user9" },
                    { 10, "user10@mail.com", false, "User 10", "password10", "user10" }
                });

            migrationBuilder.InsertData(
                table: "GroupMessages",
                columns: new[] { "Id", "GroupId", "IsDeleted", "SenderId", "SentAt", "Text" },
                values: new object[,]
                {
                    { 11, 1, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 1 from Group 1" },
                    { 12, 2, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 1 from Group 2" },
                    { 13, 3, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 1 from Group 3" },
                    { 14, 4, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 1 from Group 4" },
                    { 15, 5, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 1 from Group 5" },
                    { 21, 1, false, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 2 from Group 1" },
                    { 22, 2, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 2 from Group 2" },
                    { 23, 3, false, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 2 from Group 3" },
                    { 24, 4, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 2 from Group 4" },
                    { 25, 5, false, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 2 from Group 5" },
                    { 31, 1, false, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 3 from Group 1" },
                    { 32, 2, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 3 from Group 2" },
                    { 33, 3, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 3 from Group 3" },
                    { 34, 4, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 3 from Group 4" },
                    { 35, 5, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 3 from Group 5" },
                    { 41, 1, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 4 from Group 1" },
                    { 42, 2, false, 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 4 from Group 2" },
                    { 43, 3, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 4 from Group 3" },
                    { 44, 4, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 4 from Group 4" },
                    { 45, 5, false, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 4 from Group 5" },
                    { 51, 1, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 5 from Group 1" },
                    { 52, 2, false, 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 5 from Group 2" },
                    { 53, 3, false, 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 5 from Group 3" },
                    { 54, 4, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 5 from Group 4" },
                    { 55, 5, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 5 from Group 5" },
                    { 61, 1, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 6 from Group 1" },
                    { 62, 2, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 6 from Group 2" },
                    { 63, 3, false, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 6 from Group 3" },
                    { 64, 4, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 6 from Group 4" },
                    { 65, 5, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 6 from Group 5" },
                    { 71, 1, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 7 from Group 1" },
                    { 72, 2, false, 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 7 from Group 2" },
                    { 73, 3, false, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 7 from Group 3" },
                    { 74, 4, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 7 from Group 4" },
                    { 75, 5, false, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 7 from Group 5" },
                    { 81, 1, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 8 from Group 1" },
                    { 82, 2, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 8 from Group 2" },
                    { 83, 3, false, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 8 from Group 3" },
                    { 84, 4, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 8 from Group 4" },
                    { 85, 5, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 8 from Group 5" },
                    { 91, 1, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 9 from Group 1" },
                    { 92, 2, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 9 from Group 2" },
                    { 93, 3, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 9 from Group 3" },
                    { 94, 4, false, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 9 from Group 4" },
                    { 95, 5, false, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 9 from Group 5" },
                    { 101, 1, false, 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 10 from Group 1" },
                    { 102, 2, false, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 10 from Group 2" },
                    { 103, 3, false, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 10 from Group 3" },
                    { 104, 4, false, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 10 from Group 4" },
                    { 105, 5, false, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Message 10 from Group 5" }
                });

            migrationBuilder.InsertData(
                table: "GroupUserXREF",
                columns: new[] { "GroupId", "UserId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 2, 5 },
                    { 2, 8 },
                    { 2, 9 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 10 },
                    { 4, 7 },
                    { 5, 1 },
                    { 5, 6 }
                });

            migrationBuilder.InsertData(
                table: "MessageLikes",
                columns: new[] { "Id", "GroupMessageId", "IsDeleted", "UserId" },
                values: new object[,]
                {
                    { 113, 11, false, 3 },
                    { 115, 11, false, 5 },
                    { 116, 11, false, 6 },
                    { 117, 11, false, 7 },
                    { 118, 11, false, 8 },
                    { 119, 11, false, 9 },
                    { 120, 11, false, 10 },
                    { 123, 12, false, 3 },
                    { 124, 12, false, 4 },
                    { 125, 12, false, 5 },
                    { 126, 12, false, 6 },
                    { 127, 12, false, 7 },
                    { 130, 12, false, 10 },
                    { 131, 13, false, 1 },
                    { 132, 13, false, 2 },
                    { 133, 13, false, 3 },
                    { 136, 13, false, 6 },
                    { 138, 13, false, 8 },
                    { 141, 14, false, 1 },
                    { 145, 14, false, 5 },
                    { 146, 14, false, 6 },
                    { 149, 14, false, 9 },
                    { 151, 15, false, 1 },
                    { 153, 15, false, 3 },
                    { 154, 15, false, 4 },
                    { 156, 15, false, 6 },
                    { 160, 15, false, 10 },
                    { 212, 21, false, 2 },
                    { 213, 21, false, 3 },
                    { 214, 21, false, 4 },
                    { 215, 21, false, 5 },
                    { 217, 21, false, 7 },
                    { 220, 21, false, 10 },
                    { 221, 22, false, 1 },
                    { 223, 22, false, 3 },
                    { 224, 22, false, 4 },
                    { 226, 22, false, 6 },
                    { 229, 22, false, 9 },
                    { 230, 22, false, 10 },
                    { 232, 23, false, 2 },
                    { 235, 23, false, 5 },
                    { 236, 23, false, 6 },
                    { 237, 23, false, 7 },
                    { 240, 23, false, 10 },
                    { 241, 24, false, 1 },
                    { 243, 24, false, 3 },
                    { 245, 24, false, 5 },
                    { 249, 24, false, 9 },
                    { 252, 25, false, 2 },
                    { 254, 25, false, 4 },
                    { 255, 25, false, 5 },
                    { 257, 25, false, 7 },
                    { 311, 31, false, 1 },
                    { 314, 31, false, 4 },
                    { 315, 31, false, 5 },
                    { 316, 31, false, 6 },
                    { 317, 31, false, 7 },
                    { 319, 31, false, 9 },
                    { 328, 32, false, 8 },
                    { 329, 32, false, 9 },
                    { 330, 32, false, 10 },
                    { 335, 33, false, 5 },
                    { 336, 33, false, 6 },
                    { 338, 33, false, 8 },
                    { 339, 33, false, 9 },
                    { 340, 33, false, 10 },
                    { 341, 34, false, 1 },
                    { 343, 34, false, 3 },
                    { 344, 34, false, 4 },
                    { 345, 34, false, 5 },
                    { 348, 34, false, 8 },
                    { 349, 34, false, 9 },
                    { 351, 35, false, 1 },
                    { 352, 35, false, 2 },
                    { 354, 35, false, 4 },
                    { 356, 35, false, 6 },
                    { 360, 35, false, 10 },
                    { 412, 41, false, 2 },
                    { 414, 41, false, 4 },
                    { 416, 41, false, 6 },
                    { 417, 41, false, 7 },
                    { 418, 41, false, 8 },
                    { 422, 42, false, 2 },
                    { 425, 42, false, 5 },
                    { 426, 42, false, 6 },
                    { 428, 42, false, 8 },
                    { 429, 42, false, 9 },
                    { 432, 43, false, 2 },
                    { 433, 43, false, 3 },
                    { 435, 43, false, 5 },
                    { 436, 43, false, 6 },
                    { 438, 43, false, 8 },
                    { 441, 44, false, 1 },
                    { 442, 44, false, 2 },
                    { 447, 44, false, 7 },
                    { 449, 44, false, 9 },
                    { 453, 45, false, 3 },
                    { 454, 45, false, 4 },
                    { 457, 45, false, 7 },
                    { 458, 45, false, 8 },
                    { 512, 51, false, 2 },
                    { 514, 51, false, 4 },
                    { 516, 51, false, 6 },
                    { 517, 51, false, 7 },
                    { 518, 51, false, 8 },
                    { 520, 51, false, 10 },
                    { 521, 52, false, 1 },
                    { 525, 52, false, 5 },
                    { 527, 52, false, 7 },
                    { 530, 52, false, 10 },
                    { 532, 53, false, 2 },
                    { 533, 53, false, 3 },
                    { 535, 53, false, 5 },
                    { 536, 53, false, 6 },
                    { 537, 53, false, 7 },
                    { 541, 54, false, 1 },
                    { 543, 54, false, 3 },
                    { 545, 54, false, 5 },
                    { 546, 54, false, 6 },
                    { 548, 54, false, 8 },
                    { 549, 54, false, 9 },
                    { 551, 55, false, 1 },
                    { 552, 55, false, 2 },
                    { 553, 55, false, 3 },
                    { 554, 55, false, 4 },
                    { 557, 55, false, 7 },
                    { 611, 61, false, 1 },
                    { 614, 61, false, 4 },
                    { 615, 61, false, 5 },
                    { 616, 61, false, 6 },
                    { 621, 62, false, 1 },
                    { 624, 62, false, 4 },
                    { 627, 62, false, 7 },
                    { 630, 62, false, 10 },
                    { 631, 63, false, 1 },
                    { 633, 63, false, 3 },
                    { 635, 63, false, 5 },
                    { 637, 63, false, 7 },
                    { 638, 63, false, 8 },
                    { 644, 64, false, 4 },
                    { 647, 64, false, 7 },
                    { 649, 64, false, 9 },
                    { 653, 65, false, 3 },
                    { 654, 65, false, 4 },
                    { 658, 65, false, 8 },
                    { 660, 65, false, 10 },
                    { 711, 71, false, 1 },
                    { 715, 71, false, 5 },
                    { 716, 71, false, 6 },
                    { 718, 71, false, 8 },
                    { 721, 72, false, 1 },
                    { 722, 72, false, 2 },
                    { 724, 72, false, 4 },
                    { 725, 72, false, 5 },
                    { 729, 72, false, 9 },
                    { 731, 73, false, 1 },
                    { 732, 73, false, 2 },
                    { 737, 73, false, 7 },
                    { 738, 73, false, 8 },
                    { 740, 73, false, 10 },
                    { 741, 74, false, 1 },
                    { 742, 74, false, 2 },
                    { 743, 74, false, 3 },
                    { 744, 74, false, 4 },
                    { 746, 74, false, 6 },
                    { 748, 74, false, 8 },
                    { 749, 74, false, 9 },
                    { 752, 75, false, 2 },
                    { 754, 75, false, 4 },
                    { 755, 75, false, 5 },
                    { 756, 75, false, 6 },
                    { 811, 81, false, 1 },
                    { 812, 81, false, 2 },
                    { 813, 81, false, 3 },
                    { 814, 81, false, 4 },
                    { 817, 81, false, 7 },
                    { 818, 81, false, 8 },
                    { 819, 81, false, 9 },
                    { 820, 81, false, 10 },
                    { 822, 82, false, 2 },
                    { 823, 82, false, 3 },
                    { 824, 82, false, 4 },
                    { 826, 82, false, 6 },
                    { 827, 82, false, 7 },
                    { 829, 82, false, 9 },
                    { 832, 83, false, 2 },
                    { 835, 83, false, 5 },
                    { 836, 83, false, 6 },
                    { 837, 83, false, 7 },
                    { 845, 84, false, 5 },
                    { 846, 84, false, 6 },
                    { 848, 84, false, 8 },
                    { 849, 84, false, 9 },
                    { 850, 84, false, 10 },
                    { 851, 85, false, 1 },
                    { 852, 85, false, 2 },
                    { 853, 85, false, 3 },
                    { 913, 91, false, 3 },
                    { 915, 91, false, 5 },
                    { 916, 91, false, 6 },
                    { 917, 91, false, 7 },
                    { 918, 91, false, 8 },
                    { 927, 92, false, 7 },
                    { 928, 92, false, 8 },
                    { 931, 93, false, 1 },
                    { 932, 93, false, 2 },
                    { 935, 93, false, 5 },
                    { 938, 93, false, 8 },
                    { 940, 93, false, 10 },
                    { 942, 94, false, 2 },
                    { 947, 94, false, 7 },
                    { 950, 94, false, 10 },
                    { 953, 95, false, 3 },
                    { 956, 95, false, 6 },
                    { 957, 95, false, 7 },
                    { 958, 95, false, 8 },
                    { 960, 95, false, 10 },
                    { 1011, 101, false, 1 },
                    { 1012, 101, false, 2 },
                    { 1014, 101, false, 4 },
                    { 1015, 101, false, 5 },
                    { 1019, 101, false, 9 },
                    { 1022, 102, false, 2 },
                    { 1023, 102, false, 3 },
                    { 1028, 102, false, 8 },
                    { 1029, 102, false, 9 },
                    { 1030, 102, false, 10 },
                    { 1031, 103, false, 1 },
                    { 1032, 103, false, 2 },
                    { 1033, 103, false, 3 },
                    { 1035, 103, false, 5 },
                    { 1036, 103, false, 6 },
                    { 1038, 103, false, 8 },
                    { 1039, 103, false, 9 },
                    { 1041, 104, false, 1 },
                    { 1042, 104, false, 2 },
                    { 1044, 104, false, 4 },
                    { 1045, 104, false, 5 },
                    { 1048, 104, false, 8 },
                    { 1049, 104, false, 9 },
                    { 1050, 104, false, 10 },
                    { 1051, 105, false, 1 },
                    { 1053, 105, false, 3 },
                    { 1055, 105, false, 5 },
                    { 1058, 105, false, 8 },
                    { 1060, 105, false, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_GroupId",
                table: "GroupMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_SenderId",
                table: "GroupMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUserXREF_UserId",
                table: "GroupUserXREF",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLikes_GroupMessageId",
                table: "MessageLikes",
                column: "GroupMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLikes_UserId",
                table: "MessageLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUserXREF");

            migrationBuilder.DropTable(
                name: "MessageLikes");

            migrationBuilder.DropTable(
                name: "GroupMessages");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
