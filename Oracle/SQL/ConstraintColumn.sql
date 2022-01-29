select  owner,
        constraint_name,
        table_name,
        column_name,
        position
  FROM all_cons_columns
WHERE owner =  @OWNER

