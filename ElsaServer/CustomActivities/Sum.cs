﻿using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;

namespace ElsaServer.CustomActivities
{
    public class Sum : CodeActivity<long>
    {
        //public Sum(Variable<int> a, Variable<int> b, Variable<int> result)
        //{
        //    A = new(a);
        //    B = new(b);
        //    Result = new(result);
        //}

        public Input<long> A { get; set; } = default!;
        public Input<long> B { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var input1 = A.Get(context);
            var input2 = B.Get(context);
            var result = input1 + input2;
            context.SetResult(result);
        }
    }
}