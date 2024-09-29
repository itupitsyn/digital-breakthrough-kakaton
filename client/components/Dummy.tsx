"use client";

import { useTransitionContext } from "@/contexts/transitionContext";
import { FC } from "react";
import { Spinner } from "./Spinner";

export const Dummy: FC = () => {
  const [isPending] = useTransitionContext();

  return (
    <div className="relative flex justify-center text-2xl font-bold">
      Сначала залогиньтесь
      {isPending && <Spinner />}
    </div>
  );
};
