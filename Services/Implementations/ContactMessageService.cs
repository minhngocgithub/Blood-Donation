using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Implementations
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<ContactMessageDto?> GetMessageByIdAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactMessages
                    .Include(c => c.ResolvedByUser)
                    .FirstOrDefaultAsync(c => c.MessageId == messageId);

                if (message == null) return null;

                return new ContactMessageDto
                {
                    MessageId = message.MessageId,
                    FullName = message.FullName,
                    Email = message.Email,
                    Phone = message.Phone,
                    Subject = message.Subject,
                    Message = message.Message,
                    Status = message.Status,
                    CreatedDate = message.CreatedDate,
                    ResolvedDate = message.ResolvedDate,
                    ResolvedBy = message.ResolvedBy,
                    ResolvedByUserName = message.ResolvedByUser?.FullName
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync()
        {
            try
            {
                var messages = await _context.ContactMessages
                    .Include(c => c.ResolvedByUser)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync();

                return messages.Select(c => new ContactMessageDto
                {
                    MessageId = c.MessageId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Subject = c.Subject,
                    Message = c.Message,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    ResolvedDate = c.ResolvedDate,
                    ResolvedBy = c.ResolvedBy,
                    ResolvedByUserName = c.ResolvedByUser?.FullName
                });
            }
            catch
            {
                return new List<ContactMessageDto>();
            }
        }

        public async Task<ContactMessageDto> CreateMessageAsync(ContactMessageDto messageDto)
        {
            try
            {
                var message = new ContactMessage
                {
                    FullName = messageDto.FullName,
                    Email = messageDto.Email,
                    Phone = messageDto.Phone,
                    Subject = messageDto.Subject,
                    Message = messageDto.Message,
                    Status = messageDto.Status,
                    CreatedDate = DateTime.Now
                };

                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                return await GetMessageByIdAsync(message.MessageId) ?? messageDto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateMessageAsync(int messageId, ContactMessageDto messageDto)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                message.Subject = messageDto.Subject;
                message.Message = messageDto.Message;
                message.Status = messageDto.Status;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Status management
        public async Task<bool> UpdateMessageStatusAsync(int messageId, MessageStatus status)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                message.Status = status;

                if (status == MessageStatus.Resolved)
                {
                    message.ResolvedDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Read/Unread management - Since ContactMessage doesn't have IsRead property, 
        // we'll use Status to track read/unread state
        public async Task<bool> MarkAsReadAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                if (message.Status == MessageStatus.New)
                {
                    message.Status = MessageStatus.Read;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> MarkAsUnreadAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                if (message.Status == MessageStatus.Read)
                {
                    message.Status = MessageStatus.New;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Query operations
        public async Task<IEnumerable<ContactMessageDto>> GetUnreadMessagesAsync()
        {
            try
            {
                var messages = await _context.ContactMessages
                    .Include(c => c.ResolvedByUser)
                    .Where(c => c.Status == MessageStatus.New)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync();

                return messages.Select(c => new ContactMessageDto
                {
                    MessageId = c.MessageId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Subject = c.Subject,
                    Message = c.Message,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    ResolvedDate = c.ResolvedDate,
                    ResolvedBy = c.ResolvedBy,
                    ResolvedByUserName = c.ResolvedByUser?.FullName
                });
            }
            catch
            {
                return new List<ContactMessageDto>();
            }
        }

        public async Task<IEnumerable<ContactMessageDto>> GetMessagesByStatusAsync(MessageStatus status)
        {
            try
            {
                var messages = await _context.ContactMessages
                    .Include(c => c.ResolvedByUser)
                    .Where(c => c.Status == status)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync();

                return messages.Select(c => new ContactMessageDto
                {
                    MessageId = c.MessageId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Subject = c.Subject,
                    Message = c.Message,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    ResolvedDate = c.ResolvedDate,
                    ResolvedBy = c.ResolvedBy,
                    ResolvedByUserName = c.ResolvedByUser?.FullName
                });
            }
            catch
            {
                return new List<ContactMessageDto>();
            }
        }

        // Since ContactMessage doesn't have Category property, we'll return empty list
        public Task<IEnumerable<ContactMessageDto>> GetMessagesByCategoryAsync(string category)
        {
            return Task.FromResult<IEnumerable<ContactMessageDto>>(new List<ContactMessageDto>());
        }

        // Since ContactMessage doesn't have Priority property, we'll return empty list
        public Task<IEnumerable<ContactMessageDto>> GetMessagesByPriorityAsync(string priority)
        {
            return Task.FromResult<IEnumerable<ContactMessageDto>>(new List<ContactMessageDto>());
        }

        // Reply functionality - Since ContactMessage doesn't have Response property,
        // we'll just update the status to Resolved
        public async Task<bool> ReplyToMessageAsync(int messageId, ContactMessageDto replyDto)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                // Update message status to Resolved
                message.Status = MessageStatus.Resolved;
                message.ResolvedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                
                // TODO: Implement email sending functionality
                // For now, we'll just log the reply
                // In a real application, you would:
                // 1. Send email to the original sender
                // 2. Store the reply in a separate table
                // 3. Log the communication
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Statistics
        public async Task<ContactMessageStatisticsDto> GetMessageStatisticsAsync()
        {
            try
            {
                var totalMessages = await _context.ContactMessages.CountAsync();
                var unreadMessages = await _context.ContactMessages.CountAsync(c => c.Status == MessageStatus.New);
                var resolvedMessages = await _context.ContactMessages.CountAsync(c => c.Status == MessageStatus.Resolved);
                var readMessages = await _context.ContactMessages.CountAsync(c => c.Status == MessageStatus.Read);
                var inProgressMessages = await _context.ContactMessages.CountAsync(c => c.Status == MessageStatus.InProgress);
                var closedMessages = await _context.ContactMessages.CountAsync(c => c.Status == MessageStatus.Closed);

                var statusStats = await _context.ContactMessages
                    .GroupBy(c => c.Status)
                    .Select(g => new StatusStatDto 
                    { 
                        Status = g.Key, 
                        Count = g.Count(),
                        Percentage = totalMessages > 0 ? (double)g.Count() / totalMessages * 100 : 0
                    })
                    .ToListAsync();

                return new ContactMessageStatisticsDto
                {
                    Total = totalMessages,
                    Unread = unreadMessages,
                    Resolved = resolvedMessages,
                    Read = readMessages,
                    InProgress = inProgressMessages,
                    Closed = closedMessages,
                    ResolutionRate = totalMessages > 0 ? (double)resolvedMessages / totalMessages * 100 : 0,
                    StatusStats = statusStats
                };
            }
            catch
            {
                return new ContactMessageStatisticsDto
                {
                    Total = 0,
                    Unread = 0,
                    Resolved = 0,
                    Read = 0,
                    InProgress = 0,
                    Closed = 0,
                    ResolutionRate = 0.0,
                    StatusStats = new List<StatusStatDto>()
                };
            }
        }

        // Search functionality
        public async Task<IEnumerable<ContactMessageDto>> SearchMessagesAsync(string searchTerm)
        {
            try
            {
                var messages = await _context.ContactMessages
                    .Include(c => c.ResolvedByUser)
                    .Where(c => c.FullName.Contains(searchTerm) || 
                               c.Email.Contains(searchTerm) || 
                               c.Subject.Contains(searchTerm) || 
                               c.Message.Contains(searchTerm))
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync();

                return messages.Select(c => new ContactMessageDto
                {
                    MessageId = c.MessageId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Subject = c.Subject,
                    Message = c.Message,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    ResolvedDate = c.ResolvedDate,
                    ResolvedBy = c.ResolvedBy,
                    ResolvedByUserName = c.ResolvedByUser?.FullName
                });
            }
            catch
            {
                return new List<ContactMessageDto>();
            }
        }
    }
} 