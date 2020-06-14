using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    class PositionViewModelMapper:IPositionVewModelMapper
    {
        public PositionViewModel Map(PositionDTO positionDto)
        {
            return new PositionViewModel()
            {
                Latitude = $"{positionDto.Latitude:0.000000}",
                Longitude = $"{positionDto.Latitude:0.000000}"
            };
        }

        public PositionDTO Map(PositionViewModel positionViewModel)
        {
            return  new PositionDTO()
            {
                Latitude = double.Parse(positionViewModel.Latitude),
                Longitude = double.Parse(positionViewModel.Longitude)
            };
        }
    }
}
