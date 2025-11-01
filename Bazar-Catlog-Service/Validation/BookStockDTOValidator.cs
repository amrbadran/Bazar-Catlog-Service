using Bazar_Catlog_Service.DTOs;

namespace Bazar_Catlog_Service.Validation;

public class BookStockDTOValidator : IValidator
{
    private readonly BookStockDTO  _bookStockDTO;

    public BookStockDTOValidator(BookStockDTO bookStockDto)
    {
        _bookStockDTO = bookStockDto;
    }

    public void Validate()
    {
        var increae = _bookStockDTO.Increase;
        var decrease = _bookStockDTO.Decrease;

        if (!increae.HasValue && !decrease.HasValue)
        {
            throw new Exception("increae or decrease must have a value");
        }
        if (increae > decrease)
        {
            throw new Exception("Increase and decrease book stock exceed");
        }

        if (increae.HasValue && increae.Value < 0)
        {
            throw new Exception("Increase is negative");
        }

        if (decrease.HasValue && decrease.Value < 0)
        {
            throw new Exception("Decrease is negative");
        }
        
    }
}