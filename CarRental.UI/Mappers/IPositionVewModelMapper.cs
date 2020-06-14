using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    public interface IPositionVewModelMapper
    {
        PositionViewModel Map(PositionDTO positionDto);
        PositionDTO Map(PositionViewModel positionViewModel);
    }
}
