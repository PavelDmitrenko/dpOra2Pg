select owner,
  constraint_name, 
  constraint_type, 
  table_name, 
  search_condition, 
  r_owner, 
  r_constraint_name, 
  delete_rule, 
  status, 
  deferrable, 
  deferred, 
  validated,
  generated, 
  bad, 
  rely, 
  last_change, 
  index_owner, 
  index_name,
  invalid,
  view_related
FROM all_constraints 
WHERE OWNER =  @OWNER

