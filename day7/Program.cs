/*--- Day 7: No Space Left On Device ---
You can hear birds chirping and raindrops hitting leaves as the expedition proceeds. Occasionally, you can even hear much louder sounds in the distance; how big do the animals get out here, anyway?

The device the Elves gave you has problems with more than just its communication system. You try to run a system update:

$ system-update --please --pretty-please-with-sugar-on-top
Error: No space left on device
Perhaps you can delete some files to make space for the update?

You browse around the filesystem to assess the situation and save the resulting terminal output (your puzzle input). For example:

$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k
The filesystem consists of a tree of files (plain data) and directories (which can contain other directories or files). The outermost directory is called /. You can navigate around the filesystem, moving into or out of directories and listing the contents of the directory you're currently in.

Within the terminal output, lines that begin with $ are commands you executed, very much like some modern computers:

cd means change directory. This changes which directory is the current directory, but the specific result depends on the argument:
cd x moves in one level: it looks in the current directory for the directory named x and makes it the current directory.
cd .. moves out one level: it finds the directory that contains the current directory, then makes that directory the current directory.
cd / switches the current directory to the outermost directory, /.
ls means list. It prints out all of the files and directories immediately contained by the current directory:
123 abc means that the current directory contains a file named abc with size 123.
dir xyz means that the current directory contains a directory named xyz.
Given the commands and output in the example above, you can determine that the filesystem looks visually like this:

- / (dir)
  - a (dir)
    - e (dir)
      - i (file, size=584)
    - f (file, size=29116)
    - g (file, size=2557)
    - h.lst (file, size=62596)
  - b.txt (file, size=14848514)
  - c.dat (file, size=8504156)
  - d (dir)
    - j (file, size=4060174)
    - d.log (file, size=8033020)
    - d.ext (file, size=5626152)
    - k (file, size=7214296)
Here, there are four directories: / (the outermost directory), a and d (which are in /), and e (which is in a). These directories also contain files of various sizes.

Since the disk is full, your first step should probably be to find directories that are good candidates for deletion. To do this, you need to determine the total size of each directory. The total size of a directory is the sum of the sizes of the files it contains, directly or indirectly. (Directories themselves do not count as having any intrinsic size.)

The total sizes of the directories above can be found as follows:

The total size of directory e is 584 because it contains a single file i of size 584 and no other directories.
The directory a has total size 94853 because it contains files f (size 29116), g (size 2557), and h.lst (size 62596), plus file i indirectly (a contains e which contains i).
Directory d has total size 24933642.
As the outermost directory, / contains every file. Its total size is 48381165, the sum of the size of every file.
To begin, find all of the directories with a total size of at most 100000, then calculate the sum of their total sizes. In the example above, these directories are a and e; the sum of their total sizes is 95437 (94853 + 584). (As in this example, this process can count files more than once!)

Find all of the directories with a total size of at most 100000. What is the sum of the total sizes of those directories?
*/

/*
--- Part Two ---
Now, you're ready to choose a directory to delete.

The total disk space available to the filesystem is 70000000. To run the update, you need unused space of at least 30000000. You need to find a directory you can delete that will free up enough space to run the update.

In the example above, the total size of the outermost directory (and thus the total amount of used space) is 48381165; this means that the size of the unused space must currently be 21618835, which isn't quite the 30000000 required by the update. Therefore, the update still requires a directory with total size of at least 8381165 to be deleted before it can run.

To achieve this, you have the following options:

Delete directory e, which would increase unused space by 584.
Delete directory a, which would increase unused space by 94853.
Delete directory d, which would increase unused space by 24933642.
Delete directory /, which would increase unused space by 48381165.
Directories e and a are both too small; deleting them would not free up enough space. However, directories d and / are both big enough! Between these, choose the smallest: d, increasing unused space by 24933642.

Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update. What is the total size of that directory?
*/

using System.Diagnostics;
using System;
using System.Text.RegularExpressions;
using System.Linq; 
using System.Collections;

class Directory {
    public string name;
    public int size;

    public Directory(string _name)
    {
        name=_name;
        size=0;
    }
}

class Camp {

    
    private static Stack<Directory> directoryStack = new Stack<Directory>();

    public static void Main(string[] args) {
        Regex cd_rx = new Regex(@"\$ cd ([\w\.\/]+)",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        Regex file_rx = new Regex(@"(\d+) [\w\.]+",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        using (StringReader reader = new StringReader(STRATEGY))
        {
            int totalBigFolders = 0;        
            string? line = reader.ReadLine();
            while(line != null) {
                // are we going in/out a level?
                MatchCollection matches = cd_rx.Matches(line);
                if(matches.Count > 0) {
                    Match match = matches[0];
                    // are we going out a level?  if so, update the size of this directory
                    if(match.Groups[1].Value == "..") {
                        Directory dir = directoryStack.Pop();
                        Console.WriteLine("Left "+dir.name);
                        if(dir.size <= 100000) {
                            Console.WriteLine("Small directory "+dir.name+" of size "+dir.size);
                            totalBigFolders += dir.size;
                        } else {
                            Console.WriteLine("Large directory "+dir.name+" of size "+dir.size);
                        }
                        //update parent with my size
                        directoryStack.Peek().size += dir.size;
                        Console.WriteLine("Parent directory "+directoryStack.Peek().name+" of size "+directoryStack.Peek().size);
                    } else {
                        Console.WriteLine("Entered "+match.Groups[1].Value);
                        directoryStack.Push(new Directory(match.Groups[1].Value));
                    }
                    
                    line = reader.ReadLine();
                    continue;
                }

                // did we find a file?
                matches = file_rx.Matches(line);
                if(matches.Count > 0) {
                    Match match = matches[0];
                    // update the current directory with the size of this file
                    int size = Int32.Parse(match.Groups[1].Value);
                    directoryStack.Peek().size += size;
                    
                    line = reader.ReadLine();
                    continue;
                }

                line = reader.ReadLine();
            }

            Console.WriteLine(totalBigFolders);
        }

        
    }

    private static string STRATEGY = """
$ cd /
$ ls
dir bntdgzs
179593 cjw.jgc
110209 grbwdwsm.znn
dir hsswswtq
dir jdfwmhg
dir jlcbpsr
70323 qdtbvqjj
48606 qdtbvqjj.zdg
dir tvcr
dir vhjbjr
dir vvsg
270523 wpsjfqtn.ljt
$ cd bntdgzs
$ ls
297955 gcwcp
$ cd ..
$ cd hsswswtq
$ ls
dir bsjbvff
dir dpgvp
267138 grbwdwsm.znn
dir hldgfpvh
dir jdfwmhg
dir jtgdv
93274 ptsd.nzh
268335 qdtbvqjj.dlh
185530 qdtbvqjj.jrw
dir vcbqdj
dir wtrsg
$ cd bsjbvff
$ ls
dir dmnt
148799 grbwdwsm.znn
324931 hzmqrfc.lsd
211089 qdtbvqjj
$ cd dmnt
$ ls
221038 zht
$ cd ..
$ cd ..
$ cd dpgvp
$ ls
dir fzttpjtd
dir jdrbwrc
dir rwz
dir tssm
$ cd fzttpjtd
$ ls
149872 jdfwmhg
$ cd ..
$ cd jdrbwrc
$ ls
149973 hpgg.srm
dir ptsd
$ cd ptsd
$ ls
2594 twzf.pqq
$ cd ..
$ cd ..
$ cd rwz
$ ls
dir jdfwmhg
302808 zzlh
$ cd jdfwmhg
$ ls
229683 cdcrgcmh
218733 nhzt
$ cd ..
$ cd ..
$ cd tssm
$ ls
dir ptsd
37272 qfnnrqsh.qvg
215066 wnvjc.jqf
$ cd ptsd
$ ls
24102 bwtbht.dwq
224035 qdtbvqjj.dmp
$ cd ..
$ cd ..
$ cd ..
$ cd hldgfpvh
$ ls
316712 grbwdwsm.znn
328950 tqvgqjrr
$ cd ..
$ cd jdfwmhg
$ ls
130652 gcwcp
dir jdfwmhg
215427 lfw.zml
dir qdtbvqjj
4181 rgsvgssj.qsr
$ cd jdfwmhg
$ ls
dir bvm
dir hsswswtq
122279 qznt.jhl
dir sjw
dir zpfdtl
$ cd bvm
$ ls
22841 fbcgh.mrp
dir hsswswtq
dir hstg
41317 ndrt
dir nvmvghb
239316 ptsd
dir qtwvdtsp
98555 vzh
$ cd hsswswtq
$ ls
dir ddcjvjgf
127104 plwvb.pbj
dir ptsd
dir qhp
dir rjtrhgwh
$ cd ddcjvjgf
$ ls
135870 bwtbht.dwq
81968 gcwcp
182253 mrbh.wmc
275931 nsrqrts
322128 pfpcp
$ cd ..
$ cd ptsd
$ ls
214981 jsrlsc
dir wpbdrcw
$ cd wpbdrcw
$ ls
197849 mljfb.ggb
173586 ptsd
$ cd ..
$ cd ..
$ cd qhp
$ ls
293198 bnrgl
$ cd ..
$ cd rjtrhgwh
$ ls
224393 clrp.nst
$ cd ..
$ cd ..
$ cd hstg
$ ls
51671 gdsfpc
209216 hsswswtq
97203 jlnr
dir thdhg
57399 tssm
$ cd thdhg
$ ls
201896 jjp.wvw
$ cd ..
$ cd ..
$ cd nvmvghb
$ ls
210047 gfcrzgj
dir rqjbplv
dir rvwd
292931 sgwvcqfr.bpq
dir vtjd
$ cd rqjbplv
$ ls
105204 gcwcp
$ cd ..
$ cd rvwd
$ ls
66170 jdfwmhg
$ cd ..
$ cd vtjd
$ ls
dir ptsd
$ cd ptsd
$ ls
300524 bwtbht.dwq
$ cd ..
$ cd ..
$ cd ..
$ cd qtwvdtsp
$ ls
289574 wctgtq
$ cd ..
$ cd ..
$ cd hsswswtq
$ ls
24935 gcwcp
dir jzpbdcmc
26834 mljfb.ggb
182501 phnmlsjp.pjc
dir pttnl
dir qdtbvqjj
dir vst
$ cd jzpbdcmc
$ ls
297521 grbwdwsm.znn
dir qwc
dir zzswd
$ cd qwc
$ ls
81143 hsswswtq.rjw
54843 mjvvfsz.rgz
273051 pfwgtmtt.ccs
$ cd ..
$ cd zzswd
$ ls
216062 vlbwz.zmh
$ cd ..
$ cd ..
$ cd pttnl
$ ls
257733 mljfb.ggb
250887 pfwgtmtt.ccs
$ cd ..
$ cd qdtbvqjj
$ ls
34667 gcwcp
$ cd ..
$ cd vst
$ ls
70250 pfwgtmtt.ccs
dir zpcqhml
$ cd zpcqhml
$ ls
219936 jdfwmhg.zbm
$ cd ..
$ cd ..
$ cd ..
$ cd sjw
$ ls
152311 nqjtvzff
157117 pfwgtmtt.ccs
118226 ptsd.vsm
$ cd ..
$ cd zpfdtl
$ ls
189042 gcwcp
$ cd ..
$ cd ..
$ cd qdtbvqjj
$ ls
dir ftz
dir hvlffb
dir lzbb
53335 ptsd
dir qdtbvqjj
$ cd ftz
$ ls
dir fft
256058 gcwcp
497 hsswswtq.vqs
103941 hvtcz.fsg
171587 ljlnz.ffg
115101 mljfb.ggb
dir qdtbvqjj
$ cd fft
$ ls
58845 bwtbht.dwq
136040 gcwcp
256973 mljfb.ggb
$ cd ..
$ cd qdtbvqjj
$ ls
dir fgqhdh
304573 ntm.wmc
$ cd fgqhdh
$ ls
317143 gcwcp
26010 lsfpfdqz
$ cd ..
$ cd ..
$ cd ..
$ cd hvlffb
$ ls
6682 vjt.mcf
$ cd ..
$ cd lzbb
$ ls
dir bbvml
324162 bwtbht.dwq
dir fjs
dir pffntc
dir pnltt
dir ptsd
$ cd bbvml
$ ls
dir qdtbvqjj
dir qssdcrp
dir tssm
$ cd qdtbvqjj
$ ls
246275 qdtbvqjj.cgn
$ cd ..
$ cd qssdcrp
$ ls
274399 hsswswtq
$ cd ..
$ cd tssm
$ ls
dir ssqc
$ cd ssqc
$ ls
178904 njrssmlm.gcm
$ cd ..
$ cd ..
$ cd ..
$ cd fjs
$ ls
dir dmvnp
121967 fqlzlvwt
204348 grbwdwsm.znn
102733 jdfwmhg.qsl
240279 ptsd.jwm
228793 ptsd.nsh
dir ssm
$ cd dmvnp
$ ls
dir psj
dir zjw
$ cd psj
$ ls
170665 gcwcp
56058 lsfzc.dcp
40658 tfsllqqw.fgv
$ cd ..
$ cd zjw
$ ls
79989 fggsl.dmz
$ cd ..
$ cd ..
$ cd ssm
$ ls
106263 bwtbht.dwq
106259 jdfwmhg.qtb
6246 rwbnr.tqv
$ cd ..
$ cd ..
$ cd pffntc
$ ls
111475 qbmrdms.ldm
$ cd ..
$ cd pnltt
$ ls
dir nptfhlf
dir zngmf
$ cd nptfhlf
$ ls
223065 qrb.drh
205674 rdgfz
$ cd ..
$ cd zngmf
$ ls
61655 bwtbht.dwq
$ cd ..
$ cd ..
$ cd ptsd
$ ls
dir hrvrt
dir thwtl
$ cd hrvrt
$ ls
152296 pfwgtmtt.ccs
$ cd ..
$ cd thwtl
$ ls
156783 pfwgtmtt.ccs
323304 sltc
$ cd ..
$ cd ..
$ cd ..
$ cd qdtbvqjj
$ ls
320175 pfwgtmtt.ccs
$ cd ..
$ cd ..
$ cd ..
$ cd jtgdv
$ ls
81164 ptsd.tpj
$ cd ..
$ cd vcbqdj
$ ls
dir crng
330203 gvlrg
152022 qdtbvqjj.slq
294095 rthwj.zrf
dir vjsbf
$ cd crng
$ ls
dir gznrh
$ cd gznrh
$ ls
259458 ptsd
$ cd ..
$ cd ..
$ cd vjsbf
$ ls
47331 hlld.fzf
147103 jdfwmhg
$ cd ..
$ cd ..
$ cd wtrsg
$ ls
144344 dtcc
$ cd ..
$ cd ..
$ cd jdfwmhg
$ ls
323973 qdtbvqjj
$ cd ..
$ cd jlcbpsr
$ ls
dir htrdwm
dir jdfwmhg
dir pwmvbhsl
dir vwfdfmcp
$ cd htrdwm
$ ls
dir btn
105731 dlncqrbm.dgl
158267 gqqghldt
242513 hsswswtq.drj
dir jdfwmhg
212816 swsgtv.wbb
228996 tgll.rcs
$ cd btn
$ ls
50419 pfwgtmtt.ccs
$ cd ..
$ cd jdfwmhg
$ ls
dir bwc
$ cd bwc
$ ls
184634 cfwg
$ cd ..
$ cd ..
$ cd ..
$ cd jdfwmhg
$ ls
319749 hsswswtq
dir jdfwmhg
271619 jdfwmhg.znz
dir jhmmt
181217 mljfb.ggb
11297 rcpl.tgf
83423 zwscbcvm.ths
$ cd jdfwmhg
$ ls
267171 cts.hlf
$ cd ..
$ cd jhmmt
$ ls
84473 jdfwmhg
$ cd ..
$ cd ..
$ cd pwmvbhsl
$ ls
dir jsg
171725 mljfb.ggb
152612 qjr
dir vfsqw
$ cd jsg
$ ls
176951 jdfwmhg.fhn
284927 ljvvtw.wcq
153109 vnvtt
$ cd ..
$ cd vfsqw
$ ls
104559 htsrns.gws
$ cd ..
$ cd ..
$ cd vwfdfmcp
$ ls
291404 csmvbjlt.tdf
$ cd ..
$ cd ..
$ cd tvcr
$ ls
dir djtwv
dir hsswswtq
272845 mdds
dir ndshbjzn
65929 scpltww.twm
dir tssm
30516 zdpscm
dir zqdrdzv
$ cd djtwv
$ ls
271696 cwjj.hjp
$ cd ..
$ cd hsswswtq
$ ls
dir djngm
dir hcz
dir ptsd
$ cd djngm
$ ls
317775 ltwjzpjb.rcj
37776 qdtbvqjj.lzf
$ cd ..
$ cd hcz
$ ls
217741 pgdmr
128868 qdtbvqjj
306138 zbmrplsn
$ cd ..
$ cd ptsd
$ ls
304048 ftm
120236 mdcwvvng
$ cd ..
$ cd ..
$ cd ndshbjzn
$ ls
206408 pfwgtmtt.ccs
$ cd ..
$ cd tssm
$ ls
dir mlcnsf
dir nbgjm
204079 pdljvb
185465 rqgdmbjf.rhr
dir sfnlb
$ cd mlcnsf
$ ls
249868 fqrncwd
29146 zdz.jth
$ cd ..
$ cd nbgjm
$ ls
113314 mljfb.ggb
$ cd ..
$ cd sfnlb
$ ls
234917 tjp
$ cd ..
$ cd ..
$ cd zqdrdzv
$ ls
40790 vtdnhzm
$ cd ..
$ cd ..
$ cd vhjbjr
$ ls
dir glv
dir mvns
dir qbrnh
$ cd glv
$ ls
288849 bgvqll.sfj
259105 jdfwmhg
dir qcjlshcv
$ cd qcjlshcv
$ ls
dir nwqqjcmh
$ cd nwqqjcmh
$ ls
137244 grbwdwsm.znn
312904 mzh
dir qdtbvqjj
$ cd qdtbvqjj
$ ls
dir nlqbq
$ cd nlqbq
$ ls
307636 ptsd.vtr
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd mvns
$ ls
dir gzqlmrdh
dir qjhtlh
dir tssm
dir vthg
$ cd gzqlmrdh
$ ls
274950 mlzdqwm
$ cd ..
$ cd qjhtlh
$ ls
157835 ptsd.lqm
300380 wst.trp
$ cd ..
$ cd tssm
$ ls
15772 gcwcp
$ cd ..
$ cd vthg
$ ls
dir gdndtlnc
$ cd gdndtlnc
$ ls
3175 hsswswtq.bds
320462 mljfb.ggb
305508 mzvtzvqc
dir qdtbvqjj
154575 tssm.vgb
$ cd qdtbvqjj
$ ls
236889 drnnvh
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd qbrnh
$ ls
dir hsswswtq
4623 hsswswtq.rnf
266326 jrmq.ztg
295980 tssm.vzb
dir wnbfzd
dir zjzhncs
dir zttlggt
$ cd hsswswtq
$ ls
48277 gsqjdbhv
$ cd ..
$ cd wnbfzd
$ ls
97133 mljfb.ggb
$ cd ..
$ cd zjzhncs
$ ls
298303 gcwcp
dir ggr
113206 grbwdwsm.znn
$ cd ggr
$ ls
244876 ptsd.zvb
$ cd ..
$ cd ..
$ cd zttlggt
$ ls
dir hdbwrcm
dir mbvpd
dir mtd
dir ptsd
dir tcwqp
$ cd hdbwrcm
$ ls
267323 bwtbht.dwq
$ cd ..
$ cd mbvpd
$ ls
84087 frf.smv
$ cd ..
$ cd mtd
$ ls
158543 mljfb.ggb
$ cd ..
$ cd ptsd
$ ls
112797 vtschwnb.fnp
$ cd ..
$ cd tcwqp
$ ls
90637 lbsqcj.sfn
179097 tssm.dbl
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd vvsg
$ ls
168715 bwtbht.dwq
dir bwv
dir hsswswtq
dir lqmnjrlb
dir mmrfrj
175244 vct.tsc
dir zwvlhs
$ cd bwv
$ ls
201509 gcwcp
62815 grbwdwsm.znn
dir gwdh
dir mfdvcn
166355 pfwgtmtt.ccs
dir ptsd
169681 qdtbvqjj.fgh
250573 wvndzgv
$ cd gwdh
$ ls
306377 sphrj.pjh
$ cd ..
$ cd mfdvcn
$ ls
27796 bvclvtrm.jlf
65045 cghr.vzg
dir hsswswtq
197145 jdqztgh.pvd
$ cd hsswswtq
$ ls
298155 bwtbht.dwq
$ cd ..
$ cd ..
$ cd ptsd
$ ls
27501 grbwdwsm.znn
231999 jdnsv
113528 rmfmb.zzw
dir tssm
dir vgjfsh
$ cd tssm
$ ls
dir dndv
226375 grbwdwsm.znn
$ cd dndv
$ ls
152739 sdjrzcv.tvs
$ cd ..
$ cd ..
$ cd vgjfsh
$ ls
211409 swtbttb.vrp
170879 vvfnf.hrp
$ cd ..
$ cd ..
$ cd ..
$ cd hsswswtq
$ ls
dir qdtbvqjj
dir tssm
86418 vhsgq
$ cd qdtbvqjj
$ ls
118588 bwtbht.dwq
$ cd ..
$ cd tssm
$ ls
113460 gml.wdg
$ cd ..
$ cd ..
$ cd lqmnjrlb
$ ls
dir tssm
$ cd tssm
$ ls
dir jdfwmhg
$ cd jdfwmhg
$ ls
64663 nswd.rwc
$ cd ..
$ cd ..
$ cd ..
$ cd mmrfrj
$ ls
319070 gltlwnlt.jzw
232039 hspr
104688 hsswswtq.jsr
dir jdfwmhg
88712 jdfwmhg.zcw
dir pfr
dir prnnpwcd
45488 qdtbvqjj
dir tssm
dir wcmwrtjn
$ cd jdfwmhg
$ ls
140910 bjjhtzct.stm
$ cd ..
$ cd pfr
$ ls
289538 qdtbvqjj
217502 vvpwf
$ cd ..
$ cd prnnpwcd
$ ls
dir qdtbvqjj
$ cd qdtbvqjj
$ ls
dir pqg
dir tssm
$ cd pqg
$ ls
222392 ptsd.ggr
$ cd ..
$ cd tssm
$ ls
158252 dcnvjj.zfd
10486 jdfwmhg.qmb
4374 qdtbvqjj.vqm
254229 vgqfw
$ cd ..
$ cd ..
$ cd ..
$ cd tssm
$ ls
dir ptsd
$ cd ptsd
$ ls
173766 fvlsgqb
35658 wtc.vvd
$ cd ..
$ cd ..
$ cd wcmwrtjn
$ ls
160089 chfhpc
76202 frgpdnd.ngw
138996 jsfsfpqg.nhf
dir mlm
dir nbdbzsn
dir ptsd
278574 vrnb
$ cd mlm
$ ls
dir gqwhhmvd
dir nrzvzgrt
dir nzplht
dir zzp
$ cd gqwhhmvd
$ ls
dir ddmvjpj
dir jdfwmhg
$ cd ddmvjpj
$ ls
273423 jdfwmhg
43605 pfwgtmtt.ccs
$ cd ..
$ cd jdfwmhg
$ ls
239406 qctw.vzb
$ cd ..
$ cd ..
$ cd nrzvzgrt
$ ls
20712 gcwcp
239372 gjgdvbwb.gcz
dir hdzhl
124814 jdfwmhg
dir jfzr
295071 qwjgwqp
221611 shrzpsj.dwh
dir tssm
dir wdlsvzvl
$ cd hdzhl
$ ls
dir gfwbd
184323 hsswswtq.mln
177147 nqgqz.tnf
4680 pfwgtmtt.ccs
$ cd gfwbd
$ ls
254870 cldm.fft
301411 tssm.cvn
$ cd ..
$ cd ..
$ cd jfzr
$ ls
dir dvvflnnw
dir jdfwmhg
216389 lwtwn.ttt
201727 pfwgtmtt.ccs
107829 prphc.ncb
5816 sdvq.jvn
$ cd dvvflnnw
$ ls
24741 brtrbwh.wwd
27700 mljfb.ggb
$ cd ..
$ cd jdfwmhg
$ ls
325218 bwtbht.dwq
63718 mvl.ngz
162645 vtd.vgp
$ cd ..
$ cd ..
$ cd tssm
$ ls
60903 pfwgtmtt.ccs
332768 qdtbvqjj.jwb
$ cd ..
$ cd wdlsvzvl
$ ls
142213 vgvd
$ cd ..
$ cd ..
$ cd nzplht
$ ls
275904 hsswswtq
157369 jdfwmhg
84363 jvcvmbm.fht
dir qbjqgg
$ cd qbjqgg
$ ls
331934 gcwcp
$ cd ..
$ cd ..
$ cd zzp
$ ls
151335 flsd.zmj
dir gwlhqlp
99086 jdfwmhg.hft
$ cd gwlhqlp
$ ls
201894 glcnpqzp.jvc
$ cd ..
$ cd ..
$ cd ..
$ cd nbdbzsn
$ ls
169929 bwtbht.dwq
$ cd ..
$ cd ptsd
$ ls
128999 bwtbht.dwq
dir jtlrn
dir pszlt
dir ptjnh
dir ptsd
2981 qdtbvqjj.qcn
dir rpb
dir tcjgpqj
dir tmddnh
dir tssm
$ cd jtlrn
$ ls
124888 grbwdwsm.znn
30046 jznz.dwf
$ cd ..
$ cd pszlt
$ ls
154368 dbblsg.mzr
$ cd ..
$ cd ptjnh
$ ls
306974 grbwdwsm.znn
82840 ptsd
$ cd ..
$ cd ptsd
$ ls
dir ftjhsb
dir jdfwmhg
304012 lqgtvmrl.qbj
96971 mljfb.ggb
$ cd ftjhsb
$ ls
56965 dhgds
$ cd ..
$ cd jdfwmhg
$ ls
dir lssbmtms
dir vmwshd
$ cd lssbmtms
$ ls
95453 gcwcp
198402 mljfb.ggb
1507 mzlmp
40526 twlqhml
$ cd ..
$ cd vmwshd
$ ls
267087 pfwgtmtt.ccs
$ cd ..
$ cd ..
$ cd ..
$ cd rpb
$ ls
dir lqbchlbp
dir ptsd
$ cd lqbchlbp
$ ls
151429 ptsd.tjz
$ cd ..
$ cd ptsd
$ ls
28900 gcwcp
55920 llt
$ cd ..
$ cd ..
$ cd tcjgpqj
$ ls
dir cvdlcvq
329232 hcmj.nvp
232764 nvtmgc.qgs
108056 ptsd.gcn
39056 qdtbvqjj
91792 tssm.wqz
$ cd cvdlcvq
$ ls
46978 grbwdwsm.znn
17760 qrdbsdpj.dhm
$ cd ..
$ cd ..
$ cd tmddnh
$ ls
238434 gggvq.tfc
$ cd ..
$ cd tssm
$ ls
dir tlllv
$ cd tlllv
$ ls
198184 trmf.qqw
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd zwvlhs
$ ls
19923 gcwcp
129179 grbwdwsm.znn
214660 pghcvh
101270 ptsd.gzl
dir srjlz
$ cd srjlz
$ ls
221301 nrcg.pqw
""";
}