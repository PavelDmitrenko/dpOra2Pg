﻿SELECT table_catalog, 
			table_schema, 
			table_name, 
			table_type, 
			self_referencing_column_name,
			reference_generation,
			user_defined_type_catalog,
			user_defined_type_schema, 
			user_defined_type_name, 
			is_insertable_into, 
			is_typed, 
			commit_action
FROM 
   information_schema.tables
