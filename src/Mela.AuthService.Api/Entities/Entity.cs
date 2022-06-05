using System.ComponentModel.DataAnnotations;

namespace Mela.AuthService.Api.Entities;

/// <summary>
///     Абстрактный класс сущности
/// </summary>
public abstract class Entity
{
    /// <summary>
    ///     Идентификатор сущности
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    ///     Признак удаления
    /// </summary>
    public bool IsDeleted { get; set; }
}