"use client";

import { DarkThemeToggle, Navbar, TextInput } from "flowbite-react";
import Logo from "../assets/extremely-serious-blowing-sun.svg";

import { FC, useEffect, useState } from "react";
import { BiSearch } from "react-icons/bi";
import { useDebounce } from "@uidotdev/usehooks";
import { usePathname, useRouter } from "next/navigation";

export const UltraNavbar: FC = () => {
  const { replace } = useRouter();
  const path = usePathname();
  const [search, setSearch] = useState("");
  const searchDeb = useDebounce(search, 500);

  useEffect(() => {
    replace(path, { scroll: false });
  }, [path, replace, searchDeb]);

  console.log(searchDeb);

  return (
    <Navbar className="sticky top-0 z-10 [&>div]:gap-2">
      <Logo className="size-14" />
      <div className="dark:text-white">Ультравзрывной подбор очка</div>
      <TextInput
        icon={BiSearch}
        placeholder="Поиск"
        className="max-w-[500px] grow"
        onChange={(e) => setSearch(e.target.value)}
      />
      <DarkThemeToggle />
    </Navbar>
  );
};
