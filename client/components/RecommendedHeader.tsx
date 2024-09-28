"use client";

import { useTransitionContext } from "@/contexts/transitionContext";
import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import { FC } from "react";

export const RecommendedHeader: FC = () => {
  const { refresh } = useRouter();
  const [isPending, startTransition] = useTransitionContext();

  return (
    <div className="flex items-center justify-between gap-4">
      <h2 className="mb-2 text-xl font-medium">Рекомендации</h2>

      <Button type="button" onClick={() => startTransition(refresh)} disabled={isPending}>
        Обновить
      </Button>
    </div>
  );
};
