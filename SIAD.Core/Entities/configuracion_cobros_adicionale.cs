using System;
using System.Collections.Generic;

namespace SIAD.Core.Entities;

public partial class configuracion_cobros_adicionale
{
    public int ide { get; set; }

    public int concepto_id { get; set; }

    public int? categoria_id { get; set; }

    public bool? aplica_descuento { get; set; }

    public int? servicio_id { get; set; }
}
