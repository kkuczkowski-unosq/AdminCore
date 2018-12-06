﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmployeeRole.cs" company="AdminCore">
//   AdminCore
// </copyright>
// <summary>
//   The patient.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  /// <summary>
  ///   The patient.
  /// </summary>
  [Table("employee_role")]
  public class EmployeeRole
  {
    /// <summary>
    ///   Gets or sets the description.
    /// </summary>
    [StringLength(50)]
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    ///   Gets or sets the employee_role_id.
    /// </summary>
    [Key]
    [Column("employee_role_id")]
    public int EmployeeRoleId { get; set; }
  }
}