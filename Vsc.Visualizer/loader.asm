		ldc_p #start
in:		in_x
		stp_x
		ldc_y 0
		sub
		jiz #start
		cpx
		ldc_y 1
		add
		czp
		jmp #in
start:
