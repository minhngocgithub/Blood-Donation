using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.ViewModels.Profile;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BloodType>> GetBloodTypesAsync()
        {
            return await _context.BloodTypes.OrderBy(b => b.BloodTypeName).ToListAsync();
        }

        public async Task<ProfileViewModel> GetProfileAsync(string userId)
        {
            try
            {
                if (!int.TryParse(userId, out int id))
                {
                    throw new ArgumentException("Invalid user ID format");
                }

                var user = await _context.Users
                    .Include(u => u.DonationHistories)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                var lastDonation = user.DonationHistories
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefault();

                string province = "", district = "", ward = "", addressDetail = "";
                if (!string.IsNullOrEmpty(user.Address))
                {
                    var addressParts = user.Address.Split(',').Select(p => p.Trim()).ToList();

                    if (addressParts.Count >= 4)
                    {
                        addressDetail = addressParts[0];
                        ward = addressParts[1];
                        district = addressParts[2];
                        province = addressParts[3];
                    }
                    else
                    {
                        addressDetail = user.Address;
                    }
                }
                else
                {
                }

                return new ProfileViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Province = province,
                    District = district,
                    Ward = ward,
                    AddressDetail = addressDetail,
                    Address = user.Address,
                    Gender = user.Gender,
                    BloodType = user.BloodTypeId?.ToString(),
                    DateOfBirth = user.DateOfBirth,
                    LastDonationDate = lastDonation?.DonationDate,
                    TotalDonations = user.DonationHistories.Count
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model)
        {
            try
            {
                if (!int.TryParse(userId, out int id))
                {
                    throw new ArgumentException("Invalid user ID format");
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.FullName = model.FullName;
                user.Phone = model.Phone;
                user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;

                var addressParts = new List<string>();
                if (!string.IsNullOrEmpty(model.AddressDetail))
                    addressParts.Add(model.AddressDetail);
                if (!string.IsNullOrEmpty(model.Ward))
                    addressParts.Add(model.Ward);
                if (!string.IsNullOrEmpty(model.District))
                    addressParts.Add(model.District);
                if (!string.IsNullOrEmpty(model.Province))
                    addressParts.Add(model.Province);

                user.Address = addressParts.Any() ? string.Join(", ", addressParts) : null;

                if (!string.IsNullOrEmpty(model.BloodType) && int.TryParse(model.BloodType, out int bloodTypeId))
                {
                    user.BloodTypeId = bloodTypeId;
                }
                

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
