﻿// <auto-generated />
using System;
using LibraryManagerWeb.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20241213134748_AddedSomeDefaultValues")]
    partial class AddedSomeDefaultValues
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.AuditEntry", b =>
                {
                    b.Property<int>("AuditEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuditEntryId"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("ExtendedDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("OPeration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatingSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TimeSpent")
                        .HasPrecision(20)
                        .HasColumnType("decimal(20,2)");

                    b.Property<string>("UserAgent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuditEntryId");

                    b.HasIndex("CountryId");

                    b.ToTable("AuditEntries", t =>
                        {
                            t.HasComment("Tabla con las entradas de auditoria de la biblioteca");
                        });
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("AuthorUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DisplayName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("Name + ' ' + LastName", true);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors", null, t =>
                        {
                            t.HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca");
                        });
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("AuthorUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<string>("Sinopsis")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AI");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorUrl");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books", t =>
                        {
                            t.HasComment("Tabla para almacenar los libros existentes en esta biblioteca");
                        });
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.BookFile", b =>
                {
                    b.Property<int>("BookFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookFileId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("FormatBookformatId")
                        .HasColumnType("int");

                    b.Property<string>("InternalFilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FilePath");

                    b.HasKey("BookFileId");

                    b.HasIndex("BookId");

                    b.HasIndex("FormatBookformatId");

                    b.ToTable("BookFiles");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.BookFormat", b =>
                {
                    b.Property<int>("BookformatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookformatId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookformatId");

                    b.ToTable("BookFormats");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.BookRating", b =>
                {
                    b.Property<int>("BookRatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookRatingId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<decimal>("Starts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(3,2)")
                        .HasDefaultValue(3m);

                    b.HasKey("BookRatingId");

                    b.HasIndex("BookId");

                    b.ToTable("BookRatings", (string)null);
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Clave primaria de la tabla paises");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", null, t =>
                        {
                            t.HasComment("Tabla para guardar los paises");
                        });
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Magazine", b =>
                {
                    b.Property<int>("MagazineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MagazineId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasComment("Campo para indicar la fecha de publicación");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LoadedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("MagazineTitle")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AI");

                    b.HasKey("MagazineId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Magazines");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.PhisicalLibrary", b =>
                {
                    b.Property<int>("PhisicalLibraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhisicalLibraryId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PhisicalLibraryId");

                    b.HasIndex("CountryId");

                    b.ToTable("PhisicalLibraries", null, t =>
                        {
                            t.HasComment("Tabla para almacenar la libreria fisica donde se encuentra el libro");
                        });
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PublisherId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PublisherName");

                    b.HasKey("PublisherId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.AuditEntry", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Book", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorUrl")
                        .HasPrincipalKey("AuthorUrl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryManagerWeb.DataAccess.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.BookFile", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryManagerWeb.DataAccess.BookFormat", "Format")
                        .WithMany()
                        .HasForeignKey("FormatBookformatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Format");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.BookRating", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Magazine", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Category", "Category")
                        .WithMany("Magazines")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.PhisicalLibrary", b =>
                {
                    b.HasOne("LibraryManagerWeb.DataAccess.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryManagerWeb.DataAccess.Category", b =>
                {
                    b.Navigation("Magazines");
                });
#pragma warning restore 612, 618
        }
    }
}
