using AdvancedQuery;

using DataLib;

// compound from
CompoundFrom.CompoundFromLINQQuery("Ferrari");
CompoundFrom.CompoundFromMethodSyntax("Ferrari");
// grouping
Grouping.ShowChampionsByCountryLINQQuery();
Grouping.ShowChampionsByCountry();

// join
Join.InnerJoin();
Join.InnerJoinWithMethods();
Join.LeftOuterJoin();
Join.LeftOuterJoinWithMethods();
Join.GroupJoin();
Join.GroupJoinWithMethods();
