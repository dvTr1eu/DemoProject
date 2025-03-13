using Application.Services;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    public class AdminSeatController(ISeatService seatService, IMapper mapper) : Controller
    {
        public async Task<bool> CreateSeats(int roomId, int totalSeats)
        {
            try
            {
                List<SeatDto> seats = new List<SeatDto>();
                int seatsPerRow = 12; 
                int totalRows = (int)Math.Ceiling((double)totalSeats / seatsPerRow); 

                int seatCount = 0;
                for (int i = 0; i < totalRows; i++)
                {
                    char row = (char)('A' + i);
                    string seatType = GetSeatType(row);
                    for (int seatNum = 1; seatNum <= seatsPerRow; seatNum++)
                    {
                        if (seatCount >= totalSeats) break; 

                        seats.Add(new SeatDto()
                        {
                            RoomId = roomId,
                            SeatRow = row,
                            SeatNumber = seatNum,
                            SeatType = seatType,
                            Status = true 
                        });

                        seatCount++;
                    }
                }

                var seatsAdd = mapper.Map<List<Seat>>(seats);

                var result = await seatService.CreateRange(seatsAdd);
                return result;
            }
            catch
            {
                return false;
            }
        }

        private string GetSeatType(char row)
        {
            if (row == 'A' || row == 'B' || row == 'C' || row == 'D')
                return "Regular"; 
            return "VIP"; 
        }
    }
}
