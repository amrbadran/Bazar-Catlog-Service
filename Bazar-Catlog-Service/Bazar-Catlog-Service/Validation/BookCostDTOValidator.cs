using Bazar_Catlog_Service.DTOs;

namespace Bazar_Catlog_Service.Validation;

public class BookCostDTOValidator : IValidator
{
    private readonly BookCostDTO _bookCostDTO;

    public BookCostDTOValidator(BookCostDTO bookCostDto)
    {
        _bookCostDTO = bookCostDto;
    }

    public void Validate()
    {
        var cost = _bookCostDTO.Cost;
        if (cost == null)
        {
            throw new Exception("Book cost is null");
                
        }
        if (cost < 0)
        {
            throw new Exception("Book cost is negative");
        }
    }
}