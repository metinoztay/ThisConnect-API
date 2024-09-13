using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ThisConnect_API.Models;

public partial class Db7877Context : DbContext
{
    public Db7877Context()
    {
    }

    public Db7877Context(DbContextOptions<Db7877Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<Qr> Qrs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db7877.public.databaseasp.net; Database=db7877; User Id=db7877; Password=Qx3+5-wGo4K%; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__ATTACHMENTS");

            entity.ToTable("ATTACHMENTS");

            entity.Property(e => e.AttachmentId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("ATTACHMENT_ID");
            entity.Property(e => e.AttachmentUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ATTACHMENT_URL");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.ChatRoomId).HasName("PK__CHAT_ROOMS");

            entity.ToTable("CHAT_ROOMS");

            entity.Property(e => e.ChatRoomId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("CHAT_ROOM_ID");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.LastMessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LAST_MESSAGE_ID");
            entity.Property(e => e.Participant1Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PARTICIPANT1_ID");
            entity.Property(e => e.Participant2Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PARTICIPANT2_ID");

            entity.HasOne(d => d.LastMessage).WithMany(p => p.ChatRooms)
                .HasForeignKey(d => d.LastMessageId)
                .HasConstraintName("FK_CHAT_ROOMS_MESSAGES");

            entity.HasOne(d => d.Participant1).WithMany(p => p.ChatRoomParticipant1s)
                .HasForeignKey(d => d.Participant1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHAT_ROOMS_USERS");

            entity.HasOne(d => d.Participant2).WithMany(p => p.ChatRoomParticipant2s)
                .HasForeignKey(d => d.Participant2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHAT_ROOMS_USERS1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__MESSAGES");

            entity.ToTable("MESSAGES");

            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("MESSAGE_ID");
            entity.Property(e => e.AttachmentId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ATTACHMENT_ID");
            entity.Property(e => e.ChatRoomId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHAT_ROOM_ID");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.ReadedAt)
                .HasColumnType("datetime")
                .HasColumnName("READED_AT");
            entity.Property(e => e.RecieverUserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RECIEVER_USER_ID");
            entity.Property(e => e.SenderUserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SENDER_USER_ID");

            entity.HasOne(d => d.Attachment).WithMany(p => p.Messages)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_MESSAGES_ATTACHMENTS");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MESSAGES_CHAT_ROOMS");

            entity.HasOne(d => d.RecieverUser).WithMany(p => p.MessageRecieverUsers)
                .HasForeignKey(d => d.RecieverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MESSAGES_USERS1");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MessageSenderUsers)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MESSAGES_USERS");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("PK__OTP");

            entity.ToTable("OTP");

            entity.Property(e => e.OtpId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("OTP_ID");
            entity.Property(e => e.ExpirationTime)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRATION_TIME");
            entity.Property(e => e.OtpValue)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OTP_VALUE");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PHONE");
        });

        modelBuilder.Entity<Qr>(entity =>
        {
            entity.HasKey(e => e.QrId).HasName("PK__QRS");

            entity.ToTable("QRS");

            entity.Property(e => e.QrId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("QR_ID");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.ShareEmail).HasColumnName("SHARE_EMAIL");
            entity.Property(e => e.ShareNote).HasColumnName("SHARE_NOTE");
            entity.Property(e => e.SharePhone).HasColumnName("SHARE_PHONE");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Qrs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QRS_QRS");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__USERS");

            entity.ToTable("USERS");

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([char](36),newid()))")
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR_URL");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.LastSeenAt)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LAST_SEEN_AT");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PHONE");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SURNAME");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
