"use client";

import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import { FC } from "react";

export const RecommendedHeader: FC = () => {
  const { refresh } = useRouter();

  return (
    <div className="flex items-center justify-between gap-4">
      <h2 className="mb-2 text-xl font-medium">Рекомендации</h2>

      <Button type="button" onClick={refresh}>
        Обновить
      </Button>
    </div>
  );
};
