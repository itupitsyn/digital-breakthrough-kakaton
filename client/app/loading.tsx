"use client";

import { Spinner } from "@/components/Spinner";

export default function Loading() {
  return (
    <div className="relative mt-14 flex justify-center">
      <Spinner />
    </div>
  );
}
