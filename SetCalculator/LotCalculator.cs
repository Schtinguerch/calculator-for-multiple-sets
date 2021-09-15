using System;
using System.Collections.Generic;
using System.Linq;

namespace SetCalculator
{
    public class LotCalculator<T>
    {
        public Lot<T> Universum { get; set; }

        public Lot<T> Union(Lot<T> A, Lot<T> B)
        {
            var unionValue = new Lot<T>(A);
            var additionalFromB = (
                from b in B
                where !b.HasDuplicates(A)
                select b);

            unionValue.AddCollection(additionalFromB);
            return unionValue;
        }

        public Lot<T> Intersect(Lot<T> A, Lot<T> B)
        {
            var intersectValue = new Lot<T>(
                from a in A
                from b in B
                where a.Equals(b)
                select a);

            return intersectValue;
        }

        public Lot<T> Difference(Lot<T> A, Lot<T> B)
        {
            var differenceValue = new Lot<T>(
                from a in A
                where !a.HasDuplicates(B)
                select a);

            return differenceValue;
        }

        public Lot<T> SymDifference(Lot<T> A, Lot<T> B)
        {
            var commonItems = Intersect(A, B);

            var differenceA = new Lot<T>(
                from a in A
                where !a.HasDuplicates(commonItems)
                select a);

            var differenceB = new Lot<T>(
                from b in B
                where !b.HasDuplicates(commonItems)
                select b);

            var symDifferenceValue = Union(differenceA, differenceB);
            return symDifferenceValue;
        }

        private bool IsInUniversum(Lot<T> A)
        {
            var concidenceLot = new Lot<T>(
                from a in A
                from u in Universum
                where a.Equals(u)
                select a);

            return concidenceLot.Count == A.Count;
        }

        public Lot<T> Complement(Lot<T> A)
        {
            if (!IsInUniversum(A))
                throw new ArgumentException(
                    "Error: the Universum does not contain" +
                    "the lot, complement is impossible");

            var complementValue = Difference(Universum, A);
            return complementValue;
        }
    }
}
