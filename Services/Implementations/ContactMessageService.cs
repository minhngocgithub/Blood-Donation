using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            catch (Exception ex)
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
            catch (Exception ex)
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
                    Status = messageDto.Status ?? "New",
                    CreatedDate = DateTime.Now
                };

                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                return await GetMessageByIdAsync(message.MessageId) ?? messageDto;
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        // Status management
        public async Task<bool> UpdateMessageStatusAsync(int messageId, string status)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                message.Status = status;

                if (status == "Resolved")
                {
                    message.ResolvedDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
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

                if (message.Status == "New")
                {
                    message.Status = "Read";
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
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

                if (message.Status == "Read")
                {
                    message.Status = "New";
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
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
                    .Where(c => c.Status == "New")
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
            catch (Exception ex)
            {
                return new List<ContactMessageDto>();
            }
        }

        public async Task<IEnumerable<ContactMessageDto>> GetMessagesByStatusAsync(string status)
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
            catch (Exception ex)
            {
                return new List<ContactMessageDto>();
            }
        }

        // Since ContactMessage doesn't have Category property, we'll return empty list
        public async Task<IEnumerable<ContactMessageDto>> GetMessagesByCategoryAsync(string category)
        {
            return new List<ContactMessageDto>();
        }

        // Since ContactMessage doesn't have Priority property, we'll return empty list
        public async Task<IEnumerable<ContactMessageDto>> GetMessagesByPriorityAsync(string priority)
        {
            return new List<ContactMessageDto>();
        }

        // Reply functionality - Since ContactMessage doesn't have Response property,
        // we'll just update the status to Resolved
        public async Task<bool> ReplyToMessageAsync(int messageId, ContactMessageDto replyDto)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null) return false;

                message.Status = "Resolved";
                message.ResolvedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Statistics
        public async Task<object> GetMessageStatisticsAsync()
        {
            try
            {
                var totalMessages = await _context.ContactMessages.CountAsync();
                var unreadMessages = await _context.ContactMessages.CountAsync(c => c.Status == "New");
                var resolvedMessages = await _context.ContactMessages.CountAsync(c => c.Status == "Resolved");
                var readMessages = await _context.ContactMessages.CountAsync(c => c.Status == "Read");

                var statusStats = await _context.ContactMessages
                    .GroupBy(c => c.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                return new
                {
                    Total = totalMessages,
                    Unread = unreadMessages,
                    Resolved = resolvedMessages,
                    Read = readMessages,
                    ResolutionRate = totalMessages > 0 ? (double)resolvedMessages / totalMessages * 100 : 0,
                    StatusStats = statusStats
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Total = 0,
                    Unread = 0,
                    Resolved = 0,
                    Read = 0,
                    ResolutionRate = 0.0,
                    StatusStats = new List<object>()
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
            catch (Exception ex)
            {
                return new List<ContactMessageDto>();
            }
        }
    }
} 