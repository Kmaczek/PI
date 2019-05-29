
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Dont want to break email generation in any case.", Scope = "member", Target = "~M:Core.Domain.Logic.BinanceService.GetSymbolValuesForAccount~Core.Model.BinanceModels.BinanceVM")]