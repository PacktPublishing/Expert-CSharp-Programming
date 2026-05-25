# Nullable + Implicit Usings Migration Progress

## What changed
- Enabled <Nullable>enable</Nullable> in LiveShaping.csproj
- Enabled <ImplicitUsings>enable</ImplicitUsings> in LiveShaping.csproj
- Added <WarningsAsErrors>nullable</WarningsAsErrors> to prevent nullable regressions

## Nullable annotations applied
- Racer.Name and Racer.Team marked 
equired
- Formula1._racers changed to nullable and lazily initialized with ??=
- LapChart._lapInfo initialized with 
ull! and protected via [MemberNotNull]
- BindableObject.PropertyChanged changed to nullable event handler
- BindableObject.SetProperty now accepts string? for caller member name
- LapRacerInfo.Racer marked 
equired

## Build verification
- Final build: succeeded
- Nullable warnings: 0
- Errors: 0

## Notes
- No runtime behavior changes were introduced
